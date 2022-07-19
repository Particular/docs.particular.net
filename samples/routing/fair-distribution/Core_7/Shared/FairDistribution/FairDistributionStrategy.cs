using System;
using NServiceBus;
using NServiceBus.Routing;
using NServiceBus.Settings;

public class FairDistributionStrategy :
    DistributionStrategy
{
    Lazy<FlowManager> flowManager;

    public FairDistributionStrategy(ReadOnlySettings settings, string endpoint, DistributionStrategyScope scope)
        : base(endpoint, scope)
    {
        // FlowManager is created by the FairDistribution feature at endpoint start time
        flowManager = new Lazy<FlowManager>(settings.Get<FlowManager>);
    }

    public override string SelectDestination(DistributionContext context)
    {
        return flowManager.Value
            .FindShortestQueue(context.ReceiverAddresses);
    }
}