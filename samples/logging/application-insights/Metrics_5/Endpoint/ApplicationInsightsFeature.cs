using Microsoft.ApplicationInsights.Extensibility;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Metrics;
using System;
using System.Threading;
using System.Threading.Tasks;

class ApplicationInsightsFeature : Feature
{
    public ApplicationInsightsFeature()
    {
        DependsOn<MetricsFeature>();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var settings = context.Settings;

        #region register-probe

        var instance = Guid.NewGuid().ToString("N");

        var endpointName = settings.EndpointName();

        var telemetryConfiguration = settings.Get<TelemetryConfiguration>();

        collector = new ProbeCollector(
            telemetryConfiguration,
            endpointName,
            instance,
            context.LocalQueueAddress(),
            context.InstanceSpecificQueueAddress()
        );

        var metrics = settings.Get<MetricsOptions>();

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

        protected override Task OnStart(IMessageSession session, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        protected override Task OnStop(IMessageSession session, CancellationToken cancellationToken = default)
        {
            instance.collector.Flush();
            return Task.CompletedTask;
        }

        ApplicationInsightsFeature instance;
    }

    #endregion

    ProbeCollector collector;
}
