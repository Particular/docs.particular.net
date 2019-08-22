using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

class EstimateTimeToBreachSLA : Feature
{
    public EstimateTimeToBreachSLA()
    {
        Prerequisite(c => !c.Settings.GetOrDefault<bool>("Endpoint.SendOnly"), "EstimateTimeToBreachSLA cannot be used from a sendonly endpoint");

        DependsOn("NServiceBus.CustomChecks.CustomChecksFeature");
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var settings = context.Settings;
        var endpointSla = settings.Get<SLASettings>();
        endpointSla.EndpointName = settings.EndpointName();

        slaBreachCounter = new EstimatedTimeToSLABreachCounter(endpointSla.TimeToBreachSLA);
        context.Container.RegisterSingleton(slaBreachCounter);
        context.Container.RegisterSingleton(endpointSla);

        context.Pipeline.OnReceivePipelineCompleted(pipelineCompleted =>
        {
            slaBreachCounter?.Update(pipelineCompleted);
            return Task.FromResult(0);
        });
    }

    EstimatedTimeToSLABreachCounter slaBreachCounter;
}