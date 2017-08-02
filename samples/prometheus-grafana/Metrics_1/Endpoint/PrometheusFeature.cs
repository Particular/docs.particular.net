using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Logging;
using Prometheus;

class PrometheusFeature : Feature
{
    #region name-mapping
    Dictionary<string, string> nameMapping = new Dictionary<string, string>
    {
        // https://prometheus.io/docs/practices/naming/
        { "# of msgs successfully processed / sec", "nservicebus_success_total"},
        {"# of msgs pulled from the input queue /sec", "nservicebus_fetched_total"},
        {"# of msgs failures / sec", "nservicebus_failure_total"},
        {"Critical Time", "nservicebus_criticaltime_seconds"},
        {"Processing Time", "nservicebus_processingtime_seconds"},
    };
    #endregion

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

        var log = LogManager.GetLogger(nameof(PrometheusFeature));
        foreach (var duration in context.Durations)
        {
            if (!nameMapping.ContainsKey(duration.Name))
            {
                log.WarnFormat("Unsupported duration probe {0}", duration.Name);
                continue;
            }
            var prometheusName = nameMapping[duration.Name];
            var summary = Prometheus.Metrics.CreateSummary(prometheusName, duration.Description, "endpoint");
            duration.Register(observer: durationLength => summary.Labels(endpoint)
                .Observe(durationLength.TotalSeconds));
        }

        foreach (var signal in context.Signals)
        {
            if (!nameMapping.ContainsKey(signal.Name))
            {
                log.WarnFormat("Unsupported signal probe {0}", signal.Name);
                continue;
            }
            var prometheusName = nameMapping[signal.Name];
            var counter = Prometheus.Metrics.CreateCounter(prometheusName, signal.Description, "endpoint");
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
