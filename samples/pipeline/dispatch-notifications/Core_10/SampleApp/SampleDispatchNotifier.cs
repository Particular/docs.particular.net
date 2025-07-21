using System;
using System.Threading.Tasks;
using NServiceBus.Routing;
using NServiceBus.Transport;
using System.Collections.Generic;

#region sample-dispatch-notifier

class SampleDispatchNotifier :
    IDispatchNotifier
{
    public async Task Notify(IEnumerable<TransportOperation> operations)
    {
        foreach (var operation in operations)
        {
            await Console.Out.WriteLineAsync($"Dispatched {operation.Message.MessageId} to {Read(operation.AddressTag)}");
        }
    }

    static string Read(AddressTag addressTag)
    {
        return addressTag switch
        {
            UnicastAddressTag u => $"Unicast: {u.Destination}",
            MulticastAddressTag m => $"Multicast: {m.MessageType}",
            _ => throw new ArgumentException(message: "addressTag is not a recognized address type", paramName: nameof(addressTag))
        };
    }
}

#endregion