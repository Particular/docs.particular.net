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

    public override string SelectReceiver(string[] receiverAddresses)
    {
        return settings.Get<FlowManager>()
            .FindShortestQueue(receiverAddresses);
    }
}