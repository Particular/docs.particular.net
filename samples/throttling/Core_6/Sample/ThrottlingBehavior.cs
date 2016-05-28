using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Pipeline;
using Octokit;

#region ThrottlingBehavior
public class ThrottlingBehavior : Behavior<IInvokeHandlerContext>
{
    static ILog log = LogManager.GetLogger<Behavior<IInvokeHandlerContext>>();
    static DateTime? nextRateLimitReset;

    public override async Task Invoke(IInvokeHandlerContext context, Func<Task> next)
    {
        DateTime? rateLimitReset = nextRateLimitReset;
        if (rateLimitReset.HasValue && rateLimitReset >= DateTime.UtcNow)
        {
            log.Info($"rate limit already exceeded. Retry after {rateLimitReset} UTC");
            await DelayMessage(context, rateLimitReset.Value)
                .ConfigureAwait(false);
            return;
        }

        try
        {
            await next()
                .ConfigureAwait(false);
        }
        catch (RateLimitExceededException ex)
        {
            DateTime? nextReset = nextRateLimitReset = ex.Reset.UtcDateTime;
            log.Info($"rate limit exceeded. Limit resets resets at {nextReset} UTC");
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
        string replyAddress;
        if (context.Headers.TryGetValue(Headers.ReplyToAddress, out replyAddress))
        {
            sendOptions.RouteReplyTo(replyAddress);
        }

        return context.Send(context.MessageBeingHandled, sendOptions);
    }
}
#endregion