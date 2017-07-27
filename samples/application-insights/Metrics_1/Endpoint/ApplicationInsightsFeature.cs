using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

class ApplicationInsightsFeature : Feature
{
    public ApplicationInsightsFeature()
    {
        Defaults(s =>
        {
            options = s.EnableMetrics();
        });
        EnableByDefault();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var settings = context.Settings;

        var endpoint = settings.LogicalAddress().EndpointInstance.Endpoint;
        var discriminator = settings.LogicalAddress().EndpointInstance.Discriminator;
        var instance = Guid.NewGuid().ToString("N");

        endpoint = settings.EndpointName();
        var queue = settings.LocalAddress();

        var collector = new ApplicationInsightProbeCollector(
            endpoint,
            discriminator,
            instance,
            queue
        );

        options.RegisterObservers(collector.Register);
        context.RegisterStartupTask(new FlushAtStop(collector));

    }

    class FlushAtStop : FeatureStartupTask
    {
        public FlushAtStop(ApplicationInsightProbeCollector collector)
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

        readonly ApplicationInsightProbeCollector collector;
    }

    MetricsOptions options;
}