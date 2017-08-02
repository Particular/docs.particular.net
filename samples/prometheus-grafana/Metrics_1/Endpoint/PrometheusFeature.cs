using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using Prometheus;

class PrometheusFeature : Feature
{
    Dictionary<string, string> nameMapping = new Dictionary<string, string>
    {
        // https://prometheus.io/docs/practices/naming/
        {"# of msgs successfully processed / sec", "nservicebus_success_total"},
        {"# of msgs pulled from the input queue /sec", "nservicebus_fetched_total"},
        {"# of msgs failures / sec", "nservicebus_failure_total"},
        {"Critical Time", "nservicebus_criticaltime_seconds"},
        {"Processing Time", "nservicebus_processingtime_seconds"},
    };

    public PrometheusFeature()
    {
        Defaults(settings =>
        {
            #region enable-nsb-metrics

            metricsOptions = settings.EnableMetrics();

            #endregion
        });
        EnableByDefault();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var settings = context.Settings;

        #region register-probe

        var endpoint = settings.EndpointName();

        metricsOptions.RegisterObservers(x => RegisterProbes(x, endpoint));
        #endregion

        context.RegisterStartupTask(new MetricServerTask());
    }

    public void RegisterProbes(ProbeContext context, string endpoint)
    {
        #region observers-registration

        foreach (var duration in context.Durations)
        {
            var summary = Prometheus.Metrics.CreateSummary(nameMapping[duration.Name], duration.Description, "endpoint");
            duration.Register(observer: durationLength => summary.Labels(endpoint).Observe(durationLength.TotalMilliseconds));
        }

        foreach (var signal in context.Signals)
        {
            var counter = Prometheus.Metrics.CreateCounter(nameMapping[signal.Name], signal.Description, "endpoint");
            signal.Register(observer: () => counter.Labels(endpoint).Inc());
        }

        #endregion
    }

    #region flush-probe

    class MetricServerTask : FeatureStartupTask
    {
        readonly MetricServer metricServer = new MetricServer(port: 3030);

        protected override Task OnStart(IMessageSession session)
        {
            metricServer.Start();
            return Task.CompletedTask;
        }

        protected override Task OnStop(IMessageSession session)
        {
            metricServer.Stop();
            return Task.CompletedTask;
        }
    }

    #endregion

    MetricsOptions metricsOptions;
}
