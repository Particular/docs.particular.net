using System.Diagnostics;
using Microsoft.ApplicationInsights;
using NServiceBus;
using NServiceBus.Logging;

class ApplicationInsightProbeCollector
{
    private static readonly ILog Log = LogManager.GetLogger<ApplicationInsightProbeCollector>();
    private readonly TelemetryClient endpointTelemetry;

    public ApplicationInsightProbeCollector(string endpointName, string instanceIdentifier)
    {
        endpointTelemetry = new TelemetryClient();
        var properties = endpointTelemetry.Context.Properties;
        properties.Add("Endpoint", endpointName);
        properties.Add("EndpointInstance", instanceIdentifier);
    }

    public void Register(ProbeContext context)
    {
        Log.InfoFormat("Registering to probe context");
        foreach (var duration in context.Durations)
        {
            duration.Register(durationLength =>
            {
                // Critical & Processing time
                var s = Stopwatch.StartNew();
                endpointTelemetry.TrackMetric(duration.Name, durationLength.TotalSeconds);
                endpointTelemetry.Flush();
                Log.InfoFormat("Metric '{0}'= {1:N} took {0:N0}ms to submit.", duration.Name, durationLength.TotalSeconds, s.ElapsedMilliseconds);
            });
        }

        foreach (var signal in context.Signals)
        {
            signal.Register(() =>
            {
                // Failed, Succesful, fetched increment count
                var s = Stopwatch.StartNew();
                endpointTelemetry.TrackEvent(signal.Name);
                endpointTelemetry.Flush();
                Log.InfoFormat("Event '{0}' took {1:N0}ms to submit.", signal.Name, s.ElapsedMilliseconds);
            });
        }
    }
}