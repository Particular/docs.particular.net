using Microsoft.ApplicationInsights;
using OpenTelemetry.Metrics;

static class Extensions
{
    #region custom-meter-exporter-installation
    public static MeterProviderBuilder AddNServiceBusTelemetryClientExporter(
        this MeterProviderBuilder builder,
        TelemetryClient telemetryClient)
    {
        var exporter = new NServiceBusTelemetryClientExporter(telemetryClient);
        var reader = new PeriodicExportingMetricReader(exporter, 10000)
        {
            TemporalityPreference = MetricReaderTemporalityPreference.Delta
        };

        return builder.AddReader(reader);
    }
    #endregion
}