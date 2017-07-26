using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.ApplicationInsights;
using NServiceBus;
using NServiceBus.Logging;

class ApplicationInsightProbeCollector
{
    private static readonly ILog Log = LogManager.GetLogger<ApplicationInsightProbeCollector>();
    private readonly TelemetryClient endpointTelemetry;
    private readonly Dictionary<string, string> ProbeNameToInsightNameMapping = new Dictionary<string, string>()
    {
        { "# of msgs successfully processed / sec", "Success"},
        { "# of msgs pulled from the input queue /sec", "Fetched" },
        { "# of msgs failures / sec", "Failure" },
        { "Critical Time", "Critical Time (ms)" },
        { "Processing Time", "Processing Time (ms)" },
    };

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
                string name;
                if (!ProbeNameToInsightNameMapping.TryGetValue(duration.Name, out name)) return;
                endpointTelemetry.TrackMetric(name, durationLength.TotalMilliseconds);
            });
        }

        foreach (var signal in context.Signals)
        {
            signal.Register(() =>
            {
                string name;
                if (!ProbeNameToInsightNameMapping.TryGetValue(signal.Name, out name)) return;
                endpointTelemetry.TrackEvent(name);
            });
        }
    }

    public void Flush()
    {
        endpointTelemetry.Flush();
    }
}
