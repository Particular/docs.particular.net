using NServiceBus;
using NServiceBus.Routing;
using NServiceBus.Settings;

public class FairDistributionStrategy :
    DistributionStrategy
{
    ReadOnlySettings settings;
    FlowManager flowManager;

    public FairDistributionStrategy(ReadOnlySettings settings, string endpoint, DistributionStrategyScope scope)
        : base(endpoint, scope)
    {
        this.settings = settings;
        flowManager = settings.Get<FlowManager>();
    }

    public override string SelectReceiver(string[] receiverAddresses)
    {
        return flowManager
            .FindShortestQueue(receiverAddresses);
    }
}