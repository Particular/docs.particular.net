using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Routing;

class WeightedDistributionStrategy :
    DistributionStrategy
{
    public override UnicastRoutingTarget SelectDestination(UnicastRoutingTarget[] allInstances)
    {
        if (allInstances.Length == 0)
        {
            return null;
        }

        var endpointName = allInstances[0].Endpoint;
        var index = indexes.AddOrUpdate(endpointName, e => 0L, (e, i) => i + ShouldMoveToNextInstance(CurrentInstance(allInstances, i)));
        return CurrentInstance(allInstances, index);
    }

    // There is 1/weight chance of returning 1 (which means move to next instance).
    // Otherwise we continue sending to the current instance.
    // returns 1 if should move to next instance. 0 if should continue sending to the current one.
    #region Should-Rotate
    long ShouldMoveToNextInstance(UnicastRoutingTarget currentInstance)
    {
        var weight = GetWeight(currentInstance);
        var r = random.Next(weight);
        return r == 0 ? 1 : 0;
    }
    #endregion


    // Returns the weight of the instance based on XML config.
    // Defaults to 1 if value is not provided.
    static int GetWeight(UnicastRoutingTarget target)
    {
        string weight;
        return target.Instance != null && target.Instance.Properties.TryGetValue("weight", out weight)
            ? int.Parse(weight)
            : 1;
    }

    static UnicastRoutingTarget CurrentInstance(IList<UnicastRoutingTarget> currentAllInstances, long index) => currentAllInstances[(int) (index%currentAllInstances.Count)];

    ConcurrentDictionary<string, long> indexes = new ConcurrentDictionary<string, long>();
    Random random = new Random();
}