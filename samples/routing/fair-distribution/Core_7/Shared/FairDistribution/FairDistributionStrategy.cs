using NServiceBus;
using NServiceBus.Routing;
using NServiceBus.Settings;

public class FairDistributionStrategy :
    DistributionStrategy
{
    ReadOnlySettings settings;

    public FairDistributionStrategy(ReadOnlySettings settings, string endpoint, DistributionStrategyScope scope)
        : base(endpoint, scope)
    {
        this.settings = settings;
    }

    public override string SelectDestination(DistributionContext context)
    {
        return settings.Get<FlowManager>()
            .FindShortestQueue(context.ReceiverAddresses);
    }
}