using Microsoft.ApplicationInsights;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using Metric = OpenTelemetry.Metrics.Metric;

#region custom-meter-exporter
class NServiceBusTelemetryClientExporter : BaseExporter<Metric>
{
    readonly TelemetryClient telemetryClient;

    public NServiceBusTelemetryClientExporter(TelemetryClient telemetryClient)
    {
        this.telemetryClient = telemetryClient;
    }

    public override ExportResult Export(in Batch<Metric> batch)
    {
        foreach (var metric in batch)
        {
            if (!metric.Name.StartsWith("nservicebus.")) continue;
            if (metric.MetricType != MetricType.LongSum) continue;

            var telemetryMetric = telemetryClient.GetMetric(metric.Name);

            foreach (var dataPoint in metric.GetMetricPoints())
            {
                telemetryMetric.TrackValue(dataPoint.GetSumLong());
            }
        }
        return ExportResult.Success;
    }
}
#endregion