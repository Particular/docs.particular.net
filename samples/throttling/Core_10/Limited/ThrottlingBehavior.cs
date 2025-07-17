using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Pipeline;
using Octokit;

#region ThrottlingBehavior
public class ThrottlingBehavior(ILogger<ThrottlingBehavior> logger) :
    Behavior<IInvokeHandlerContext>
{
    static DateTime? nextRateLimitReset;

    public override async Task Invoke(IInvokeHandlerContext context, Func<Task> next)
    {
        var rateLimitReset = nextRateLimitReset;
        if (rateLimitReset.HasValue && rateLimitReset >= DateTime.UtcNow)
        {
            var localTime = rateLimitReset?.ToLocalTime();
            logger.LogInformation("Rate limit exceeded. Retry after {RateLimitReset} UTC ({LocalTime} local).", rateLimitReset, localTime);
            await DelayMessage(context, rateLimitReset.Value);
            return;
        }

        try
        {
            await next();
        }
        catch (RateLimitExceededException exception)
        {
            var nextReset = nextRateLimitReset = exception.Reset.UtcDateTime;
            var localTime = nextReset?.ToLocalTime();
            logger.LogInformation("Rate limit exceeded. Limit resets at {NextReset} UTC ({LocalTime} local).", nextReset, localTime);
            await DelayMessage(context, nextReset.Value);
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