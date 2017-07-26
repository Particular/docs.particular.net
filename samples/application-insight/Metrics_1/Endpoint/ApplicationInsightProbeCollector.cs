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
                Log.InfoFormat("Received metric {0} = {1:N}", duration.Name, durationLength.TotalSeconds);

                var s = Stopwatch.StartNew();
                endpointTelemetry.TrackMetric(duration.Name, durationLength.TotalSeconds);
                Log.InfoFormat("TrackMetric duration: {0:N0}ms", s.ElapsedMilliseconds);
            });
        }

        foreach (var signal in context.Signals)
        {
            signal.Register(() =>
            {
                Log.InfoFormat("Received signal {0}", signal.Name);
                var s = Stopwatch.StartNew();
                endpointTelemetry.TrackEvent(signal.Name);
                Log.InfoFormat("TrackEvent duration: {0:N0}ms", s.ElapsedMilliseconds);
            });
        }
    }
}