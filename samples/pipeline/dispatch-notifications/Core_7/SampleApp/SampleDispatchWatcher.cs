using System;
using System.Threading.Tasks;
using NServiceBus.Routing;
using NServiceBus.Transport;
using System.Collections.Generic;

#region sample-dispatch-watcher

class SampleDispatchWatcher :
    IWatchDispatches
{
    public Task Notify(IEnumerable<TransportOperation> operations)
    {
        foreach (var operation in operations)
        {
            Console.WriteLine($"Dispatched {operation.Message.MessageId} to {Read(operation.AddressTag)}");
        }
        return Task.CompletedTask;
    }

    string Read(AddressTag addressTag)
    {
        if (addressTag is UnicastAddressTag u)
        {
            return $"Unicast: {u.Destination}";
        }
        if (addressTag is MulticastAddressTag m)
        {
            return $"Multicast: {m.MessageType}";
        }
        throw new ArgumentException(
            message: "addressTag is not a recognized address type",
            paramName: nameof(addressTag));
    }
}

#endregion