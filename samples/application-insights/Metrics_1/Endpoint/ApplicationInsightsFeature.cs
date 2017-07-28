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

        #region register-probe

        var discriminator = settings.LogicalAddress().EndpointInstance.Discriminator;
        var instance = Guid.NewGuid().ToString("N");

        var endpoint = settings.EndpointName();
        var queue = settings.LocalAddress();

        collector = new ApplicationInsightsProbeCollector(
            endpoint,
            discriminator,
            instance,
            queue
        );

        metricsOptions.RegisterObservers(collector.RegisterProbes);
        #endregion

        SetupServiceLevelAgreementViolationCountdownMetric(context);
        context.RegisterStartupTask(new CleanupAtStop(this));
    }

    void SetupServiceLevelAgreementViolationCountdownMetric(FeatureConfigurationContext context)
    {
        var settings = context.Settings;
        TimeSpan endpointSla;

        if (!settings.TryGet(ApplicationInsightsSettings.EndpointSLAKey, out endpointSla)) return;

        var ceiling = settings.Get<TimeSpan>(ApplicationInsightsSettings.EndpointSLACeilingKey);
        var counterInstanceName = settings.EndpointName();

        slaBreachCounter = new EstimatedTimeToSLABreachCounter(endpointSla, ceiling, collector.RegisterServiceLevelAgreementViolation);

        context.Pipeline.OnReceivePipelineCompleted(pipelineCompleted =>
        {
            slaBreachCounter?.Update(pipelineCompleted);
            return Task.CompletedTask;
        });
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
            instance.slaBreachCounter?.Dispose();
            instance.collector.Flush();
            return Task.CompletedTask;
        }

        ApplicationInsightsFeature instance;
    }

    #endregion

    MetricsOptions metricsOptions;
    ApplicationInsightsProbeCollector collector;
    EstimatedTimeToSLABreachCounter slaBreachCounter;
}
