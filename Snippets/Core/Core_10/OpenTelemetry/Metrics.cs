namespace Core.OpenTelemetry;

using global::OpenTelemetry;
using global::OpenTelemetry.Metrics;

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