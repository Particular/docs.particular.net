using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NServiceBus;
using NServiceBus.Routing;

class FlowManager
{
    public long GetNextMarker(string transportAddress)
    {
        var f = data.GetOrAdd(transportAddress, s => new FlowData());
        return f.GenerateMarker();
    }

    public void Acknowledge(string key, string endpoint, int endpointInstanceHash, long markerValue)
    {
        FlowData f;
        if (data.TryGetValue(key, out f))
        {
            f.Acknowledge(endpointInstanceHash, markerValue);
        }
    }

    #region GetLeastBusy
    public int? GetLeastBusyInstanceHash(IEnumerable<UnicastRoutingTarget> allInstances)
    {
        FlowData best = null;

        foreach (var hash in allInstances.Where(t => t.Instance != null).Select(t => t.Instance.ToString().GetHashCode()))
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

    FlowData Compare(FlowData best, FlowData candidate)
    {
        if (best == null || candidate.MessagesInFlight < best.MessagesInFlight)
        {
            return candidate;
        }
        if (candidate.MessagesInFlight == best.MessagesInFlight)
        {
            return random.Next(2) == 0 ? best : candidate;
        }
        return best;
    }

    ConcurrentDictionary<string, FlowData> data = new ConcurrentDictionary<string, FlowData>();
    Random random = new Random();

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