using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NServiceBus;
using NServiceBus.Routing;

class WeightedDistributionStrategy : DistributionStrategy
{
    /// <summary>
    /// Selects destination instances from all known instances of a given endpoint based on their relative weight.
    /// </summary>
    public override IEnumerable<UnicastRoutingTarget> SelectDestination(IList<UnicastRoutingTarget> currentAllInstances)
    {
        if (currentAllInstances.Count == 0)
        {
            return Enumerable.Empty<UnicastRoutingTarget>();
        }
        var endpointName = currentAllInstances[0].Endpoint.ToString();
        var index = indexes.AddOrUpdate(endpointName, e => 0L, (e, i) => i + ShouldMoveToNextInstance(CurrentInstance(currentAllInstances, i)));
        return new[]
        {
                CurrentInstance(currentAllInstances, index)
            };
    }

    /// <summary>
    /// There is 1/weight chance of returning 1 (which means move to next instance). Otherwise we continue sending to the current instance.
    /// </summary>
    /// <param name="currentInstance">Current instance.</param>
    /// <returns>1 if should move to next instance. 0 if should continue sending to the current one.</returns>
    #region Should-Rotate
    long ShouldMoveToNextInstance(UnicastRoutingTarget currentInstance)
    {
        var weight = GetWeight(currentInstance);
        var r = random.Next(weight);
        return r == 0 ? 1 : 0;
    }
    #endregion

    /// <summary>
    /// Returns the weight of the instance based on XML config. Defaults to 1 if value is not provided.
    /// </summary>
    /// <param name="target">Endpoint instance.</param>
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