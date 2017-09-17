using System;
using System.Linq;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using Octokit;

#region ThrottlingBehavior
public class ThrottlingBehavior :
    IBehavior<IncomingContext>
{
    IBus bus;
    static ILog log = LogManager.GetLogger<ThrottlingBehavior>();
    static DateTime? nextRateLimitReset;

    public ThrottlingBehavior(IBus bus)
    {
        this.bus = bus;
    }

    public void Invoke(IncomingContext context, Action next)
    {
        var rateLimitReset = nextRateLimitReset;
        if (rateLimitReset.HasValue && rateLimitReset >= DateTime.UtcNow)
        {
            var localTime = rateLimitReset?.ToLocalTime();
            log.Info($"Rate limit exceeded. Retry after {rateLimitReset} UTC ({localTime} local).");
            DelayMessage(context, rateLimitReset.Value);
            return;
        }

        try
        {
            next();
        }
        catch (RateLimitExceededException exception)
        {
            var nextReset = nextRateLimitReset = exception.Reset.UtcDateTime;
            var localTime = nextReset?.ToLocalTime();
            log.Info($"Rate limit exceeded. Limit resets at {nextReset} UTC ({localTime} local).");
            DelayMessage(context, nextReset.Value);
        }
    }

    void DelayMessage(IncomingContext context, DateTime deliverAt)
    {
        var message = context.LogicalMessages.Single().Instance;
        // maintain the original ReplyTo address
        if (context.PhysicalMessage.Headers.TryGetValue(Headers.ReplyToAddress, out var replyAddress))
        {
            bus.SetMessageHeader(message, Headers.ReplyToAddress, replyAddress);
        }
        // delay the message to the specified delivery date
        bus.Defer(deliverAt, message);
    }
}
#endregion