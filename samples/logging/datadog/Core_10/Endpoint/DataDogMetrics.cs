using System.Collections.Generic;
using System.Linq;
using NServiceBus;
using NServiceBus.Configuration.AdvancedExtensibility;
using StatsdClient;

public static class DataDogMetrics
{
    static string endpointName;

    public static void Setup(EndpointConfiguration endpointConfiguration)
    {
        var metricOptions = endpointConfiguration.EnableMetrics();
        endpointName = endpointConfiguration.GetSettings().EndpointName();

        #region setup-datadog-client

        var dogstatsdConfig = new StatsdConfig
        {
            StatsdServerName = "127.0.0.1",
            StatsdPort = 8125
        }; //Datadog agent default address, port

        DogStatsd.Configure(dogstatsdConfig);

        #endregion

        #region datadog-enable-nsb-metrics

        metricOptions.RegisterObservers(register: probeContext =>
        {
            foreach (var duration in probeContext.Durations)
            {
                if (!nameMapping.ContainsKey(duration.Name))
                {
                    continue;
                }
                duration.Register((ref DurationEvent @event) =>
                {
                    var statName = ComposeStatName(duration.Name);
                    var tags = ComposeTags(@event.MessageType);
                    DogStatsd.Timer(statName, @event.Duration.TotalMilliseconds, tags: tags);
                });
            }

            foreach (var signal in probeContext.Signals)
            {
                if (!nameMapping.ContainsKey(signal.Name))
                {
                    continue;
                }
                signal.Register((ref SignalEvent @event) =>
                {
                    var statName = ComposeStatName(signal.Name);
                    var tags = ComposeTags(@event.MessageType);
                    DogStatsd.Increment(statName, tags: tags);
                });
            }
        });

        #endregion
    }

    static string ComposeStatName(string eventName)
    {
        nameMapping.TryGetValue(eventName, out var mappedName);
        return mappedName;
    }

    static string[] ComposeTags(string messageType)
    {
        var tags = new List<string>
            {
                "endpoint:" + endpointName
            };

        if (!string.IsNullOrEmpty(messageType))
        {
            var fullMessageName = messageType.Split(',')[0];
            tags.Add("messagetype_fullname:" + fullMessageName);

            var shortMessageName = fullMessageName.Split('.').Last();
            tags.Add("messagetype_name:" + shortMessageName);
        }

        return tags.ToArray();
    }

    static readonly Dictionary<string, string> nameMapping = new()
    {
            {"# of msgs successfully processed / sec", "nservicebus.processed"},
            {"# of msgs pulled from the input queue /sec", "nservicebus.fetched"},
            {"# of msgs failures / sec", "nservicebus.failed"},
            {"Critical Time", "nservicebus.critical_time"},
            {"Processing Time", "nservicebus.processing_time"},
            {"Retries", "nservicebus.retries"},
        };
}