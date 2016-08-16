using System.Linq;
using NServiceBus;
using NServiceBus.Routing;
using NServiceBus.Settings;

public class FairDistributionStrategy :
    DistributionStrategy
{
    ReadOnlySettings settings;

    public FairDistributionStrategy(ReadOnlySettings settings)
    {
        this.settings = settings;
    }

    public override UnicastRoutingTarget SelectDestination(UnicastRoutingTarget[] allInstances)
    {
        var hash = settings.Get<FlowManager>()
            .GetLeastBusyInstanceHash(allInstances);

        var leastBusyInstance = allInstances.FirstOrDefault(i =>
        {
            return i.Instance != null &&
                   i.Instance.ToString().GetHashCode() == hash;
        });
        return leastBusyInstance ?? allInstances.First();
    }
}