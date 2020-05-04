using System;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Extensibility;
using NServiceBus;
using NServiceBus.Features;

class ApplicationInsightsFeature : Feature
{
    public ApplicationInsightsFeature()
    {
        Defaults(settings =>
        {
            #region enable-nsb-metrics

            metrics = settings.EnableMetrics();

            #endregion
        });
        EnableByDefault();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var settings = context.Settings;

        #region register-probe

        var logicalAddress = settings.LogicalAddress();
        var discriminator = logicalAddress.EndpointInstance.Discriminator;
        var instance = Guid.NewGuid().ToString("N");

        var endpoint = settings.EndpointName();
        var queue = settings.LocalAddress();

        var telemetryConfiguration = settings.Get<TelemetryConfiguration>();

        collector = new ProbeCollector(
            telemetryConfiguration,
            endpoint,
            discriminator,
            instance,
            queue
        );

        metrics.RegisterObservers(collector.RegisterProbes);
        #endregion

        context.RegisterStartupTask(new CleanupAtStop(this));
    }

    #region flush-probe

    class CleanupAtStop : FeatureStartupTask
    {
        public CleanupAtStop(ApplicationInsightsFeature instance)
        {
            this.instance = instance;
        }

        protected override Task OnStart(IMessageSession session)
        {
            return Task.CompletedTask;
        }

        protected override Task OnStop(IMessageSession session)
        {
            instance.collector.Flush();
            return Task.CompletedTask;
        }

        ApplicationInsightsFeature instance;
    }

    #endregion

    MetricsOptions metrics;
    ProbeCollector collector;
}
