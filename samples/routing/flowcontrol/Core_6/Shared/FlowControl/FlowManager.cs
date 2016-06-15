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
            f.Acknowledge(endpoint, endpointInstanceHash, markerValue);
        }
    }

    public int? GetLeastBusyInstanceHash(string endpoint, IEnumerable<EndpointInstance> allInstances)
    {
        var bestValue = long.MaxValue;
        int? bestHash = null;

        foreach (var hash in allInstances.Select(o => o.ToString().GetHashCode()))
        {
            var f = data.Values.FirstOrDefault(o => o.InstanceHash == hash);
            if (f == null) //We don't track this instance yet
            {
                return hash;
            }
            if (!bestHash.HasValue || f.MessagesInFlight < bestValue)
            {
                bestValue = f.MessagesInFlight;
                bestHash = f.InstanceHash;
            }
        }

        return bestHash;
    }

    ConcurrentDictionary<string, FlowData> data = new ConcurrentDictionary<string, FlowData>();

    class FlowData
    {
        string endpoint;
        int instanceHash;
        long lastAckedMarker;
        long lastGeneratedMarker;

        public int InstanceHash => instanceHash;

        public string Endpoint => endpoint;
        public long MessagesInFlight => lastGeneratedMarker - lastAckedMarker;

        public long GenerateMarker()
        {
            return Interlocked.Increment(ref lastGeneratedMarker);
        }

        public void Acknowledge(string endpoint, int instanceHash, long markerValue)
        {
            this.endpoint = endpoint;
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