using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using NServiceBus;

class ProbeCollector
{
    TelemetryClient telemetryClient;

    Dictionary<string, string> probeNameToAiNameMap = new Dictionary<string, string>
    {
        {"# of msgs successfully processed / sec", "Success"},
        {"# of msgs pulled from the input queue /sec", "Fetched"},
        {"# of msgs failures / sec", "Failure"},
        {"Critical Time", "Critical Time (ms)"},
        {"Processing Time", "Processing Time (ms)"},
    };

    public ProbeCollector(TelemetryConfiguration telemetryConfiguration, string endpointName, string discriminator, string instanceIdentifier, string queue)
    {
        #region telemetry-client
        telemetryClient = new TelemetryClient(telemetryConfiguration);
        var properties = telemetryClient.Context.GlobalProperties;
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
            if (!probeNameToAiNameMap.TryGetValue(duration.Name, out var name))
            {
                continue;
            }
            duration.Register(
                observer: (ref DurationEvent @event) =>
                {
                    var milliseconds = @event.Duration.TotalMilliseconds;
                    var telemetry = new MetricTelemetry(name, milliseconds);
                    telemetryClient.TrackMetric(telemetry);
                });
        }

        foreach (var signal in context.Signals)
        {
            if (!probeNameToAiNameMap.TryGetValue(signal.Name, out var name))
            {
                continue;
            }
            signal.Register(
                observer: (ref SignalEvent @event) =>
                {
                    telemetryClient.TrackEvent(new EventTelemetry(name));
                });
        }

        #endregion
    }

    public void Flush()
    {
        telemetryClient.Flush();
    }
}
