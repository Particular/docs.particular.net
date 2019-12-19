using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using StatsdClient;

class DatadogFeature : Feature
{
    MetricsOptions _metricsOptions;

    Dictionary<string, string> _nameMapping = new Dictionary<string, string>
    {
        {"# of msgs successfully processed / sec", "Success"},
        {"# of msgs pulled from the input queue /sec", "Fetched"},
        {"# of msgs failures / sec", "Failures"},
        {"Critical Time", "Critical Time"},
        {"Processing Time", "Processing Time"},
        {"Retries", "Retries"},
    };

    public DatadogFeature()
    {
        Defaults(settings =>
        {
            _metricsOptions = settings.EnableMetrics();
        });
        EnableByDefault();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        #region setup-datadog-client

        var dogstatsdConfig = new StatsdConfig
        {
            StatsdServerName = "127.0.0.1",
            StatsdPort = 8125,
        }; //Datadog agent default address, port

        DogStatsd.Configure(dogstatsdConfig);

        #endregion

        #region datadog-enable-nsb-metrics

        _metricsOptions.RegisterObservers(register: probeContext =>
            {
                foreach (var duration in probeContext.Durations)
                {
                    if (!_nameMapping.ContainsKey(duration.Name))
                    {
                        continue;
                    }
                    duration.Register((ref DurationEvent @event) =>
                    {
                        var statName = ComposeStatName(duration.Name, @event.MessageType);
                        DogStatsd.Timer(statName, @event.Duration.TotalMilliseconds);
                    });
                }

                foreach (var signal in probeContext.Signals)
                {
                    if (!_nameMapping.ContainsKey(signal.Name))
                    {
                        continue;
                    }
                    signal.Register((ref SignalEvent @event) =>
                    {
                        var statName = ComposeStatName(signal.Name, @event.MessageType);
                        DogStatsd.Increment(statName);
                    });
                }
            });

        #endregion
    }

    private string ComposeStatName(string eventName, string messageType)
    {
        _nameMapping.TryGetValue(eventName, out var mappedName);
        return $"{messageType.Split(',')[0]}-{mappedName}";
    }
}
