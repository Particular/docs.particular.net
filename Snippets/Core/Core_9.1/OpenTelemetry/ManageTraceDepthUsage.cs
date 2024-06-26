using System.Threading.Tasks;
using NServiceBus;

namespace Core9_1;

public static class ManageTraceDepthUsage
{
    async static Task RequestStartNewTrace(IPipelineContext context)
    {
        #region opentelemetry-sendoptions-start-new-trace
        var options = new SendOptions();
        options.StartNewTraceOnReceive();
        var message = new MyMessage();
        await context.Send(message, options);
        #endregion
    }

    async static Task RequestContinueExistingTrace(IPipelineContext context)
    {
        #region opentelemetry-publishoptions-continue-trace
        var options = new PublishOptions();
        options.ContinueExistingTraceOnReceive();
        var message = new MyEvent();
        await context.Publish(message, options);
        #endregion
    }

    class MyMessage
    {
    }

    class MyEvent
    {
    }
}