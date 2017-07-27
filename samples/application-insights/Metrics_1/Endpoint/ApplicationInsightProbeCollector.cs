using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.ApplicationInsights;
using NServiceBus;
using NServiceBus.Logging;

class ApplicationInsightProbeCollector
{
    static ILog log = LogManager.GetLogger<ApplicationInsightProbeCollector>();
    TelemetryClient endpointTelemetry;

    Dictionary<string, string> probeNameToInsightNameMapping = new Dictionary<string, string>
    {
        {"# of msgs successfully processed / sec", "Success"},
        {"# of msgs pulled from the input queue /sec", "Fetched"},
        {"# of msgs failures / sec", "Failure"},
        {"Critical Time", "Critical Time (ms)"},
        {"Processing Time", "Processing Time (ms)"},
    };

    public ApplicationInsightProbeCollector(string endpointName, string discriminator, string instanceIdentifier, string queue)
    {
        endpointTelemetry = new TelemetryClient();
        var properties = endpointTelemetry.Context.Properties;
        properties.Add("Endpoint", endpointName);
        properties.Add("EndpointInstance", instanceIdentifier);
        properties.Add("MachineName", Environment.MachineName);
        properties.Add("HostName", Dns.GetHostName());
        properties.Add("EndpointDiscriminator", discriminator);
        properties.Add("EndpointQueue", queue);
    }

    public void Register(ProbeContext context)
    {
        log.InfoFormat("Registering to probe context");
        foreach (var duration in context.Durations)
        {
            string name;
            if (probeNameToInsightNameMapping.TryGetValue(duration.Name, out name))
            {
                duration.Register(
                    observer: durationLength =>
                    {
                        endpointTelemetry.TrackMetric(name, durationLength.TotalMilliseconds);
                    });
            }
        }

        foreach (var signal in context.Signals)
        {
            string name;
            if (probeNameToInsightNameMapping.TryGetValue(signal.Name, out name))
            {
                signal.Register(
                    observer: () =>
                    {
                        endpointTelemetry.TrackEvent(name);
                    });
            }
        }
    }

    public void Flush()
    {
        endpointTelemetry.Flush();
    }
}
