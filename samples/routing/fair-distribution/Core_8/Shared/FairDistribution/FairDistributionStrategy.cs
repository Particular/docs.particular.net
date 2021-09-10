using NServiceBus;
using NServiceBus.Routing;
using NServiceBus.Settings;
using System;

public class FairDistributionStrategy :
    DistributionStrategy
{
    IReadOnlySettings settings;
    Lazy<FlowManager> flowManager;

    public FairDistributionStrategy(IReadOnlySettings settings, string endpoint, DistributionStrategyScope scope)
        : base(endpoint, scope)
    {
        this.settings = settings;
        flowManager = new Lazy<FlowManager>(() => settings.Get<FlowManager>());
    }

    public override string SelectDestination(DistributionContext context)
    {
        return flowManager.Value
            .FindShortestQueue(context.ReceiverAddresses);
    }
}