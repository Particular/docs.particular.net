using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Pipeline;
using Octokit;

#region ThrottlingBehavior
public class ThrottlingBehavior :
    Behavior<IInvokeHandlerContext>
{
    static ILog log = LogManager.GetLogger<ThrottlingBehavior>();
    static DateTime? nextRateLimitReset;

    public override async Task Invoke(IInvokeHandlerContext context, Func<Task> next)
    {
        var rateLimitReset = nextRateLimitReset;
        if (rateLimitReset.HasValue && rateLimitReset >= DateTime.UtcNow)
        {
            var localTime = rateLimitReset?.ToLocalTime();
            log.Info($"Rate limit exceeded. Retry after {rateLimitReset} UTC ({localTime} local).");
            await DelayMessage(context, rateLimitReset.Value)
                .ConfigureAwait(false);
        }

        try
        {
            await next()
                .ConfigureAwait(false);
        }
        catch (RateLimitExceededException exception)
        {
            var nextReset = nextRateLimitReset = exception.Reset.UtcDateTime;
            var localTime = nextReset?.ToLocalTime();
            log.Info($"Rate limit exceeded. Limit resets at {nextReset} UTC ({localTime} local).");
            await DelayMessage(context, nextReset.Value)
                .ConfigureAwait(false);
        }
    }

    Task DelayMessage(IInvokeHandlerContext context, DateTime deliverAt)
    {
        var sendOptions = new SendOptions();

        // delay the message to the specified delivery date
        sendOptions.DoNotDeliverBefore(deliverAt);
        // send message to this endpoint
        sendOptions.RouteToThisEndpoint();
        // maintain the original ReplyTo address
        if (context.Headers.TryGetValue(Headers.ReplyToAddress, out var replyAddress))
        {
            sendOptions.RouteReplyTo(replyAddress);
        }

        return context.Send(context.MessageBeingHandled, sendOptions);
    }
}
#endregion