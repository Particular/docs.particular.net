using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus;
using NServiceBus.Routing;
using NServiceBus.Settings;

public class ToLeastBusyDistributionStrategy : DistributionStrategy
{
    ReadOnlySettings settings;

    public ToLeastBusyDistributionStrategy(ReadOnlySettings settings)
    {
        this.settings = settings;
    }

    public override IEnumerable<UnicastRoutingTarget> SelectDestination(IList<UnicastRoutingTarget> allInstances)
    {
        var hash = settings.Get<FlowManager>()
            .GetLeastBusyInstanceHash(allInstances.First().Endpoint.ToString(), allInstances
            .Where(i => i.Instance != null)
            .Select(i => i.Instance));

        var leastBusyInstance = allInstances.FirstOrDefault(i => i.Instance != null && i.Instance.ToString().GetHashCode() == hash);
        yield return leastBusyInstance ?? allInstances.First();
    }
}