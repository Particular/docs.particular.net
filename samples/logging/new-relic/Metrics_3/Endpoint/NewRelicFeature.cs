using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Features;
using static NewRelic.Api.Agent.NewRelic;

class NewRelicFeature : Feature
{
    public NewRelicFeature()
    {
        Defaults(settings =>
        {
            #region newrelic-enable-nsb-metrics

            metricsOptions = settings.EnableMetrics();

            #endregion
        });
        EnableByDefault();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var settings = context.Settings;

        #region newrelic-name-mapping

        var endpointName = settings.EndpointName();

        var nameMapping = new Dictionary<string, string>
        {
            // https://docs.newrelic.com/docs/agents/manage-apm-agents/agent-data/collect-custom-metrics
            {"# of msgs successfully processed / sec", FormatMetric("Success_Total", endpointName)},
            {"# of msgs pulled from the input queue /sec", FormatMetric("Fetched_Total", endpointName)},
            {"# of msgs failures / sec", FormatMetric("Failure_Total", endpointName)},
            {"Critical Time", FormatMetric("CriticalTime_Seconds", endpointName)},
            {"Processing Time", FormatMetric("ProcessingTime_Seconds", endpointName)},
        };
        #endregion

        #region newrelic-register-probe

        metricsOptions.RegisterObservers(
            register: probeContext =>
            {
                RegisterProbes(probeContext, endpointName, nameMapping);
            });

        #endregion
    }

    public void RegisterProbes(ProbeContext context, string endpointName, Dictionary<string, string> nameMapping)
    {
        #region newrelic-observers-registration

        foreach (var duration in context.Durations)
        {
            duration.Register((ref DurationEvent @event) =>
            {
                nameMapping.TryGetValue(duration.Name, out var mappedName);
                var newRelicName = string.Format(mappedName ?? FormatMetric(duration.Name, endpointName), Normalize(@event.MessageType));
                RecordResponseTimeMetric(newRelicName, Convert.ToInt64(@event.Duration.TotalMilliseconds));
            });
        }

        foreach (var signal in context.Signals)
        {
            signal.Register((ref SignalEvent @event) =>
            {
                nameMapping.TryGetValue(signal.Name, out var mappedName);
                var newRelicName = string.Format(mappedName ?? FormatMetric(signal.Name, endpointName), Normalize(@event.MessageType));
                RecordMetric(newRelicName, 1);
            });
        }

        #endregion
    }

    static string Normalize(string name)
    {
        var result = name.Split(SplitComma, StringSplitOptions.RemoveEmptyEntries);
        if (result.Length > 0)
        {
            name = result[0];
        }
        return name.Replace(" ", "_").Replace(".", "_").Replace("-", "_");
    }

    static string FormatMetric(string name, string prefix)
    {
        return Normalize($"Custom/NServiceBus/{prefix}/{{0}}/{name}");
    }

    static string[] SplitComma = new[] {","};
    MetricsOptions metricsOptions;
}