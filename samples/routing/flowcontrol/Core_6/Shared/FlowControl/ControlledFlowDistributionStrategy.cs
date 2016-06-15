using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus;
using NServiceBus.Routing;
using NServiceBus.Settings;

public class ControlledFlowDistributionStrategy : DistributionStrategy
{
    ReadOnlySettings settings;

    public ControlledFlowDistributionStrategy(ReadOnlySettings settings)
    {
        this.settings = settings;
    }

    public override IEnumerable<UnicastRoutingTarget> SelectDestination(IEnumerable<UnicastRoutingTarget> allInstances)
    {
        var instances = allInstances.ToArray(); //Replaced by IList in next beta of core

        var hash = settings.Get<FlowManager>().GetLeastBusyInstanceHash(instances.First().Endpoint.ToString(), instances.Where(i => i.Instance != null).Select(i => i.Instance));
        var leastBusyInstance = instances.FirstOrDefault(i => i.Instance != null && i.Instance.ToString().GetHashCode() == hash);
        yield return leastBusyInstance ?? instances.First();
    }
}