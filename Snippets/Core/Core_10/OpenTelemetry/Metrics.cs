namespace Core9
{
    using OpenTelemetry;
    using OpenTelemetry.Metrics;

    public class Metrics
    {
        public static void EnableMetrics()
        {
            #region opentelemetry-enablemeters

            var meterProviderProvider = Sdk.CreateMeterProviderBuilder()
                .AddMeter("NServiceBus.Core.Pipeline.Incoming")
                // ... Add other meters
                // ... Add exporters
                .Build();

            #endregion
        }
    }
}