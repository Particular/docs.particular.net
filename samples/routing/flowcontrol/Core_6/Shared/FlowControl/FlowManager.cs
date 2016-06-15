using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NServiceBus.Routing;

class FlowManager
{
    public long GetNextMarker(string transportAddress)
    {
        var f = data.GetOrAdd(transportAddress, s => new FlowData());
        return f.GenerateMarker();
    }

    public void Acknowledge(string transportAddress, string endpoint, int endpointInstanceHash, long markerValue)
    {
        FlowData f;
        if (data.TryGetValue(transportAddress, out f))
        {
            f.Acknowledge(endpointInstanceHash, markerValue);
        }
    }

    #region GetLeastBusy
    public int? GetLeastBusyInstanceHash(string endpoint, IEnumerable<EndpointInstance> allInstances)
    {
        FlowData best = null;

        foreach (var hash in allInstances.Select(o => o.ToString().GetHashCode()))
        {
            var candidate = data.Values.FirstOrDefault(o => o.InstanceHash == hash);
            if (candidate == null) //We don't track this instance yet so assume it has shortest queue.
            {
                return hash;
            }
            best = Compare(best, candidate);
        }
        return best?.InstanceHash;
    }
    #endregion

    static FlowData Compare(FlowData best, FlowData candidate)
    {
        if (best == null || candidate.MessagesInFlight < best.MessagesInFlight)
        {
            best = candidate;
        }
        return best;
    }

    ConcurrentDictionary<string, FlowData> data = new ConcurrentDictionary<string, FlowData>();

    class FlowData
    {
        int instanceHash;
        long lastAckedMarker;
        long lastGeneratedMarker;

        public int InstanceHash => instanceHash;

        public long MessagesInFlight => lastGeneratedMarker - lastAckedMarker;

        public long GenerateMarker()
        {
            return Interlocked.Increment(ref lastGeneratedMarker);
        }

        public void Acknowledge(int instanceHash, long markerValue)
        {
            this.instanceHash = instanceHash;
            InterlockedExchangeIfGreaterThan(ref lastAckedMarker, markerValue);
        }

        static void InterlockedExchangeIfGreaterThan(ref long location, long newValue)
        {
            long initialValue;
            do
            {
                initialValue = location;
                if (initialValue >= newValue)
                {
                    return;
                }
            }
            while (Interlocked.CompareExchange(ref location, newValue, initialValue) != initialValue);
        }
    }
}