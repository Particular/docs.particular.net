using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

class ApplicationInsightsFeature : Feature
{
    public ApplicationInsightsFeature()
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

        var discriminator = settings.LogicalAddress().EndpointInstance.Discriminator;
        var instance = Guid.NewGuid().ToString("N");

        var endpoint = settings.EndpointName();
        var queue = settings.LocalAddress();

        var collector = new ApplicationInsightsProbeCollector(
            endpoint,
            discriminator,
            instance,
            queue
        );

        metricsOptions.RegisterObservers(collector.Register);
        context.RegisterStartupTask(new FlushAtStop(collector));

    }

    class FlushAtStop : FeatureStartupTask
    {
        public FlushAtStop(ApplicationInsightsProbeCollector collector)
        {
            this.collector = collector;
        }

        protected override Task OnStart(IMessageSession session)
        {
            return Task.CompletedTask;
        }

        protected override Task OnStop(IMessageSession session)
        {
            collector.Flush();
            return Task.CompletedTask;
        }

        readonly ApplicationInsightsProbeCollector collector;
    }

    MetricsOptions metricsOptions;
}