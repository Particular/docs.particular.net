using System.Collections.Generic;
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

    public override IEnumerable<UnicastRoutingTarget> SelectDestination(IList<UnicastRoutingTarget> allInstances)
    {
        var hash = settings.Get<FlowManager>()
            .GetLeastBusyInstanceHash(allInstances);

        var leastBusyInstance = allInstances.FirstOrDefault(i =>
        {
            return i.Instance != null &&
                   i.Instance.ToString().GetHashCode() == hash;
        });
        if (leastBusyInstance == null)
        {
            yield return allInstances.First();
        }
        else
        {
            yield return leastBusyInstance;
        }
    }
}