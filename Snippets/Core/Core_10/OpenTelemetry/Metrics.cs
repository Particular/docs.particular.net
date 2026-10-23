namespace Core.OpenTelemetry;

using System;
using System.Collections.Generic;
using global::OpenTelemetry;
using global::OpenTelemetry.Metrics;

public class Metrics
{
    public static void EnableMetrics()
    {
        #region opentelemetry-enablemeters

        var meterProviderProvider = Sdk.CreateMeterProviderBuilder()
            // Matches NServiceBus.Core.Pipeline.Incoming, NServiceBus.Core, and the other NServiceBus meters
            .AddMeter("NServiceBus.*")
            // ... Add other meters
            // ... Add exporters
            .Build();

        #endregion
    }

    public static void FilterMetrics(MeterProviderBuilder metrics)
    {
        #region opentelemetry-metrics-filter-view

        // Only these NServiceBus metrics are collected; everything else the meter emits is dropped.
        var enabledNServiceBusMetrics = new HashSet<string>(StringComparer.Ordinal)
        {
            "nservicebus.messaging.deserialize_time",
            "nservicebus.messaging.serialize_time",
        };

        metrics.AddMeter("NServiceBus.*")
            .AddView(instrument =>
                instrument.Meter.Name.StartsWith("NServiceBus", StringComparison.Ordinal)
                && !enabledNServiceBusMetrics.Contains(instrument.Name)
                    ? MetricStreamConfiguration.Drop
                    : null);

        #endregion
    }
}