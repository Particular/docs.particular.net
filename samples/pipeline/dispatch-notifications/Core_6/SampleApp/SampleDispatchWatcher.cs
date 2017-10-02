using System;
using System.Threading.Tasks;
using NServiceBus.Routing;
using NServiceBus.Transport;
using System.Collections.Generic;
using NServiceBus.Logging;

#region sample-dispatch-watcher
class SampleDispatchWatcher :
    IWatchDispatches
{
    static ILog log = LogManager.GetLogger<SampleDispatchWatcher>();

    public Task Notify(IEnumerable<TransportOperation> operations)
    {
        foreach (var operation in operations)
        {
            log.Info($"Dispatched {operation.Message.MessageId} to {Read(operation.AddressTag)}");
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