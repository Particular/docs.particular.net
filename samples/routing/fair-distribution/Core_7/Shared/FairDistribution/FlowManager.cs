using System;
using System.Collections.Concurrent;
using System.Threading;

class FlowManager
{
    public long GetNextMarker(string transportAddress)
    {
        var f = data.GetOrAdd(transportAddress, address => new FlowData(address));
        return f.GenerateMarker();
    }

    public void Acknowledge(string receiverAddress, long markerValue)
    {
        if (data.TryGetValue(receiverAddress, out var f))
        {
            f.Acknowledge(markerValue);
        }
    }

    #region GetLeastBusy

    public string FindShortestQueue(string[] receiverAddresses)
    {
        FlowData best = null;

        foreach (var address in receiverAddresses)
        {
            // This instance is not yet tracked, so assume it has shortest queue.
            if (!data.TryGetValue(address, out var candidate))
            {
                return address;
            }
            best = Compare(best, candidate);
        }
        return best.Address;
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
        long lastAckedMarker;
        long lastGeneratedMarker;

        public FlowData(string transportAddress)
        {
            Address = transportAddress;
        }

        public string Address { get; }

        public long MessagesInFlight => lastGeneratedMarker - lastAckedMarker;

        public long GenerateMarker()
        {
            return Interlocked.Increment(ref lastGeneratedMarker);
        }

        public void Acknowledge(long markerValue)
        {
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
            } while (Interlocked.CompareExchange(ref location, newValue, initialValue) != initialValue);
        }
    }
}