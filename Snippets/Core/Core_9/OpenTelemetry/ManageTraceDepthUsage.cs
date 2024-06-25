namespace Core9;

public static ManageTraceDepthUsage
{
    async Task RequestStartNewTrace(IPipelineContext context)
    {
        #region opentelemetry-sendoptions-start-new-trace
        var options = new SendOptions();
        options.StartNewTraceOnReceive();
        var message = new MyMessage();
        await context.Send(message, options);
        #endregion
    }

    async Task RequestContinueExistingTrace(IPipelineContext context)
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