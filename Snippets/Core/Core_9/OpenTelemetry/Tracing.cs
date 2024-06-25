namespace Core9
{
    using OpenTelemetry;
    using OpenTelemetry.Trace;

    public static class Tracing
    {
        public static void EnableTracing()
        {
            #region opentelemetry-enabletracing

            var tracingProviderBuilder = Sdk.CreateTracerProviderBuilder()
                .AddSource("NServiceBus.Core")
                // ... Add other trace sources
                // ... Add exporters
                .Build();

            #endregion
        }
    }

    class Usage
    {
        async Task RequestStartNewTrace(IPipelineContext context)
        {
            #region OpentelemetrySendoptionsStartNewTrace
            var options = new SendOptions();
            options.StartNewTraceOnReceive();
            var message = new MyMessage();
            await context.Send(message, options);
            #endregion
        }

        async Task RequestContinueExistingTrace(IPipelineContext context)
        {
            #region OpentelemetryPublishoptionsContinueTrace
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
}