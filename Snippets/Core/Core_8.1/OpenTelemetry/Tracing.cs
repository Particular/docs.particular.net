namespace Core_8
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
}