using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using NServiceBus;

class ApplicationInsightsProbeCollector
{
    TelemetryClient endpointTelemetry;

    Dictionary<string, string> probeNameToAiNameMap = new Dictionary<string, string>
    {
        {"# of msgs successfully processed / sec", "Success"},
        {"# of msgs pulled from the input queue /sec", "Fetched"},
        {"# of msgs failures / sec", "Failure"},
        {"Critical Time", "Critical Time (ms)"},
        {"Processing Time", "Processing Time (ms)"},
    };

    public ApplicationInsightsProbeCollector(string endpointName, string discriminator, string instanceIdentifier, string queue)
    {
        #region telemetry-client

        endpointTelemetry = new TelemetryClient();
        var properties = endpointTelemetry.Context.Properties;
        properties.Add("Endpoint", endpointName);
        properties.Add("EndpointInstance", instanceIdentifier);
        properties.Add("MachineName", Environment.MachineName);
        properties.Add("HostName", Dns.GetHostName());
        properties.Add("EndpointDiscriminator", discriminator);
        properties.Add("EndpointQueue", queue);

        #endregion
    }

    public void RegisterProbes(ProbeContext context)
    {
        #region observers-registration

        foreach (var duration in context.Durations)
        {
            string name;
            if (probeNameToAiNameMap.TryGetValue(duration.Name, out name))
            {
                duration.Register(
                    observer: durationLength => endpointTelemetry.TrackMetric(
                        new MetricTelemetry(name, durationLength.TotalMilliseconds)));
            }
        }

        foreach (var signal in context.Signals)
        {
            string name;
            if (probeNameToAiNameMap.TryGetValue(signal.Name, out name))
            {
                signal.Register(
                    observer: () => endpointTelemetry.TrackEvent(new EventTelemetry(name)));
            }
        }

        #endregion
    }

    public void RegisterServiceLevelAgreementViolation(TimeSpan value)
    {
        endpointTelemetry.TrackMetric(new MetricTelemetry("SLA violation countdown (h)", value.TotalHours));
    }

    public void Flush()
    {
        endpointTelemetry.Flush();
    }
}
