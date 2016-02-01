using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Pipeline;
using Octokit;

namespace ThroughputThrottlingDemo
{
    public class ThrottlingBehavior : Behavior<IInvokeHandlerContext>
    {
        private static DateTime? nextRateLimitReset;

        public override async Task Invoke(IInvokeHandlerContext context, Func<Task> next)
        {
            DateTime? rateLimitReset = nextRateLimitReset;
            if (rateLimitReset.HasValue && rateLimitReset >= DateTime.UtcNow)
            {
                Console.WriteLine($"rate limit already exceeded. Retry after {rateLimitReset} UTC");
                await DelayMessage(context, rateLimitReset.Value).ConfigureAwait(false);
                return;
            }

            try
            {
                await next().ConfigureAwait(false);
            }
            catch (RateLimitExceededException ex)
            {
                DateTime? nextReset = nextRateLimitReset = ex.Reset.UtcDateTime;
                Console.WriteLine($"rate limit exceeded. Limit resets resets at {nextReset} UTC");
                await DelayMessage(context, nextReset.Value).ConfigureAwait(false);
            }
        }

        private static Task DelayMessage(IInvokeHandlerContext context, DateTime deliverAt)
        {
            SendOptions sendOptions = new SendOptions();
            sendOptions.RouteToLocalEndpointInstance();
            sendOptions.DoNotDeliverBefore(deliverAt);
            return context.Send(context.MessageBeingHandled, sendOptions);
        }
    }
}