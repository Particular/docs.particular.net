using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Logging;
using NServiceBus.Routing;

class RoutingInfoSubscriber :
    FeatureStartupTask
{
    static ILog log = LogManager.GetLogger<RoutingInfoSubscriber>();

    UnicastRoutingTable routingTable;
    UnicastSubscriberTable subscriberTable;
    TimeSpan sweepPeriod;
    TimeSpan heartbeatTimeout;
    ConcurrentDictionary<string, EndpointInstanceInfo> instanceInformation = new ConcurrentDictionary<string, EndpointInstanceInfo>();
    Timer sweepTimer;
    IMessageSession messageSession;

    public RoutingInfoSubscriber(UnicastRoutingTable routingTable, UnicastSubscriberTable subscriberTable,  TimeSpan sweepPeriod, TimeSpan heartbeatTimeout)
    {
        this.routingTable = routingTable;
        this.subscriberTable = subscriberTable;
        this.sweepPeriod = sweepPeriod;
        this.heartbeatTimeout = heartbeatTimeout;
    }

    protected override Task OnStart(IMessageSession context)
    {
        messageSession = context;
        sweepTimer = new Timer(state =>
        {
            foreach (var info in instanceInformation)
            {
                if (!info.Value.Sweep(DateTime.UtcNow, heartbeatTimeout))
                {
                    log.Info($"Instance {info.Key} deactivated (heartbeat timeout).");
                }
            }
        }, null, sweepPeriod + sweepPeriod, sweepPeriod);
        return Task.CompletedTask;
    }

    protected override Task OnStop(IMessageSession session)
    {
        using (var waitHandle = new ManualResetEvent(false))
        {
            sweepTimer.Dispose(waitHandle);
            waitHandle.WaitOne();
        }
        return Task.CompletedTask;
    }

    public Task OnRemoved(Entry entry)
    {
        var deserializedData = JsonConvert.DeserializeObject<RoutingInfo>(entry.Data);

        log.Info($"Instance {deserializedData.TransportAddress} removed from routing tables.");

        UpdateCaches(deserializedData.TransportAddress, null, new Type[0], new Type[0]);

        EndpointInstanceInfo removed;
        instanceInformation.TryRemove(deserializedData.TransportAddress, out removed);
        return Task.CompletedTask;
    }

    public Task OnChanged(Entry entry)
    {
        var deserializedData = JsonConvert.DeserializeObject<RoutingInfo>(entry.Data);

        var handledCommands =
            deserializedData.HandledCommandTypes.Select(x => Type.GetType(x, false))
                .Where(x => x != null)
                .ToArray();

        var handledEvents =
            deserializedData.HandledEventTypes.Select(x => Type.GetType(x, false))
                .Where(x => x != null)
                .ToArray();

        var instanceInfo = instanceInformation.GetOrAdd(deserializedData.TransportAddress, new EndpointInstanceInfo());
        if (deserializedData.Active)
        {
            instanceInfo.Activate(deserializedData.Timestamp);
            log.Debug($"Instance {deserializedData.TransportAddress} active (heartbeat).");
        }
        else
        {
            instanceInfo.Deactivate();
            log.Info($"Instance {deserializedData.TransportAddress} deactivated.");
        }
        UpdateCaches(deserializedData.TransportAddress, UnicastRoute.CreateFromPhysicalAddress(deserializedData.TransportAddress, deserializedData.EndpointName) , handledCommands, handledEvents);
        return Task.CompletedTask;
    }

    void UpdateCaches(string key, UnicastRoute route, Type[] handledCommands, Type[] handledEvents)
    {
        routingTable.AddOrReplaceRoutes(key, handledCommands.Select(c => new RouteTableEntry(c, route)).ToList());
        subscriberTable.AddOrReplaceRoutes(key, handledEvents.Select(e => new RouteTableEntry(e, route)).ToList());
    }
}
