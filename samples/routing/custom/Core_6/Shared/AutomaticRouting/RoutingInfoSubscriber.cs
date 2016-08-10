using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Logging;
using NServiceBus.Routing;
using NServiceBus.Routing.MessageDrivenSubscriptions;

class RoutingInfoSubscriber :
    FeatureStartupTask
{
    static ILog log = LogManager.GetLogger<RoutingInfoSubscriber>();

    UnicastRoutingTable routingTable;
    EndpointInstances endpointInstances;
    IReadOnlyCollection<Type> messageTypesHandledByThisEndpoint;
    Publishers publishers;
    TimeSpan sweepPeriod;
    TimeSpan heartbeatTimeout;
    Dictionary<Type, HashSet<string>> endpointMap = new Dictionary<Type, HashSet<string>>();
    Dictionary<string, HashSet<EndpointInstance>> instanceMap = new Dictionary<string, HashSet<EndpointInstance>>();
    Dictionary<EndpointInstance, EndpointInstanceInfo> instanceInformation = new Dictionary<EndpointInstance, EndpointInstanceInfo>();
    Dictionary<Type, string> publisherMap = new Dictionary<Type, string>();
    Timer sweepTimer;
    IMessageSession messageSession;

    public RoutingInfoSubscriber(UnicastRoutingTable routingTable, EndpointInstances endpointInstances, IReadOnlyCollection<Type> messageTypesHandledByThisEndpoint, Publishers publishers, TimeSpan sweepPeriod, TimeSpan heartbeatTimeout)
    {
        this.routingTable = routingTable;
        this.endpointInstances = endpointInstances;
        this.messageTypesHandledByThisEndpoint = messageTypesHandledByThisEndpoint;
        this.publishers = publishers;
        this.sweepPeriod = sweepPeriod;
        this.heartbeatTimeout = heartbeatTimeout;
    }

    protected override Task OnStart(IMessageSession context)
    {
        messageSession = context;

        #region AddDynamic
        routingTable.AddDynamic((types, bag) => FindEndpoint(types));
        endpointInstances.AddDynamic(FindInstancesTask);
        publishers.AddDynamic(FindPublisher);
        #endregion

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
        return Task.FromResult(0);
    }

    protected override Task OnStop(IMessageSession session)
    {
        using (var waitHandle = new ManualResetEvent(false))
        {
            sweepTimer.Dispose(waitHandle);
            waitHandle.WaitOne();
        }
        return Task.FromResult(0);
    }

    public async Task OnRemoved(Entry entry)
    {
        var deserializedData = JsonConvert.DeserializeObject<RoutingInfo>(entry.Data);
        var instanceName = new EndpointInstance(deserializedData.EndpointName, deserializedData.Discriminator, deserializedData.InstanceProperties);

        log.Info($"Instance {instanceName} removed from routing tables.");

        await UpdateCaches(instanceName, new Type[0], new Type[0])
            .ConfigureAwait(false);

        instanceInformation.Remove(instanceName);
    }

    public Task OnChanged(Entry entry)
    {
        var deserializedData = JsonConvert.DeserializeObject<RoutingInfo>(entry.Data);
        var instanceName = new EndpointInstance(deserializedData.EndpointName, deserializedData.Discriminator, deserializedData.InstanceProperties);

        var handledTypes =
            deserializedData.HandledMessageTypes.Select(x => Type.GetType(x, false))
                .Where(x => x != null)
                .ToArray();

        var publishedTypes =
            deserializedData.PublishedMessageTypes.Select(x => Type.GetType(x, false))
                .Where(x => x != null)
                .ToArray();

        EndpointInstanceInfo instanceInfo;
        if (!instanceInformation.TryGetValue(instanceName, out instanceInfo))
        {
            var newInstanceInformation = new Dictionary<EndpointInstance, EndpointInstanceInfo>(instanceInformation);
            instanceInfo = new EndpointInstanceInfo();
            newInstanceInformation[instanceName] = instanceInfo;
            instanceInformation = newInstanceInformation;
        }
        if (deserializedData.Active)
        {
            instanceInfo.Activate(deserializedData.Timestamp);
            log.Debug($"Instance {instanceName} active (heartbeat).");
        }
        else
        {
            instanceInfo.Deactivate();
            log.Info($"Instance {instanceName} deactivated.");
        }
        return UpdateCaches(instanceName, handledTypes, publishedTypes);
    }

    async Task UpdateCaches(EndpointInstance instanceName, Type[] handledTypes, Type[] publishedTypes)
    {
        var newInstanceMap = BuildNewInstanceMap(instanceName);
        var newEndpointMap = BuildNewEndpointMap(instanceName.Endpoint, handledTypes, endpointMap);
        var newPublisherMap = BuildNewPublisherMap(instanceName, publishedTypes, publisherMap);
        LogChangesToEndpointMap(endpointMap, newEndpointMap);
        LogChangesToInstanceMap(instanceMap, newInstanceMap);
        var toSubscribe = LogChangesToPublisherMap(publisherMap, newPublisherMap).ToArray();
        instanceMap = newInstanceMap;
        endpointMap = newEndpointMap;
        publisherMap = newPublisherMap;

        foreach (var type in toSubscribe.Intersect(messageTypesHandledByThisEndpoint))
        {
            await messageSession.Subscribe(type)
                .ConfigureAwait(false);
        }
    }

    static IEnumerable<Type> LogChangesToPublisherMap(Dictionary<Type, string> publisherMap, Dictionary<Type, string> newPublisherMap)
    {
        foreach (var addedType in newPublisherMap.Keys.Except(publisherMap.Keys))
        {
            log.Info($"Added {newPublisherMap[addedType]} as publisher of {addedType}.");
            yield return addedType;
        }

        foreach (var removedType in publisherMap.Keys.Except(newPublisherMap.Keys))
        {
            log.Info($"Removed {publisherMap[removedType]} as publisher of {removedType}.");
        }
    }

    static void LogChangesToInstanceMap(Dictionary<string, HashSet<EndpointInstance>> instanceMap, Dictionary<string, HashSet<EndpointInstance>> newInstanceMap)
    {
        foreach (var addedEndpoint in newInstanceMap.Keys.Except(instanceMap.Keys))
        {
            log.Info($"Added endpoint {addedEndpoint} with instances {FormatSet(newInstanceMap[addedEndpoint])}");
        }

        foreach (var removedEndpoint in instanceMap.Keys.Except(newInstanceMap.Keys))
        {
            log.Info($"Removed endpoint {removedEndpoint} with instances {FormatSet(instanceMap[removedEndpoint])}");
        }

        foreach (var existingEndpoint in instanceMap.Keys.Intersect(newInstanceMap.Keys))
        {
            var addedInstances = newInstanceMap[existingEndpoint].Except(instanceMap[existingEndpoint]).ToArray();
            var removedInstances = instanceMap[existingEndpoint].Except(newInstanceMap[existingEndpoint]).ToArray();
            if (addedInstances.Any())
            {
                log.Info($"Added instances {FormatSet(addedInstances)} to endpoint {existingEndpoint}");
            }
            if (removedInstances.Any())
            {
                log.Info($"Removed instances {FormatSet(removedInstances)} from endpoint {existingEndpoint}");
            }
        }
    }

    static void LogChangesToEndpointMap(Dictionary<Type, HashSet<string>> endpointMap, Dictionary<Type, HashSet<string>> newEndpointMap)
    {
        foreach (var addedType in newEndpointMap.Keys.Except(endpointMap.Keys))
        {
            log.Info($"Added route for {addedType.Name} to {FormatSet(newEndpointMap[addedType])}");
        }

        foreach (var removedType in endpointMap.Keys.Except(newEndpointMap.Keys))
        {
            log.Info($"Removed route for {removedType.Name} to {FormatSet(newEndpointMap[removedType])}");
        }

        foreach (var existingType in endpointMap.Keys.Intersect(newEndpointMap.Keys))
        {
            var newSet = newEndpointMap[existingType];
            var currentSet = endpointMap[existingType];
            var addedEndpoints = newSet.Except(currentSet).ToArray();
            var removedEndpoints = currentSet.Except(newSet).ToArray();
            if (addedEndpoints.Any())
            {
                log.Info($"Added route for {existingType.Name} to {FormatSet(addedEndpoints)}");
            }
            if (removedEndpoints.Any())
            {
                log.Info($"Removed route for {existingType.Name} to {FormatSet(removedEndpoints)}");
            }
        }
    }

    static string FormatSet(IEnumerable<object> set)
    {
        return string.Join(", ", set.Select(o => $"[{o}]"));
    }

    static Dictionary<Type, string> BuildNewPublisherMap(EndpointInstance endpointInstance, Type[] publishedTypes, Dictionary<Type, string> publisherMap)
    {
        var newPublisherMap = new Dictionary<Type, string>();
        var endpointName = endpointInstance.Endpoint;
        foreach (var pair in publisherMap)
        {
            if (pair.Value != endpointName)
            {
                newPublisherMap[pair.Key] = pair.Value;
            }
        }
        foreach (var publishedType in publishedTypes)
        {
            if (newPublisherMap.ContainsKey(publishedType))
            {
                log.Warn($"More than one endpoint advertises publishing of {publishedType}: {newPublisherMap[publishedType]}, {endpointInstance.Endpoint}");
            }
            else
            {
                newPublisherMap[publishedType] = endpointName;
            }
        }
        return newPublisherMap;
    }

    Dictionary<string, HashSet<EndpointInstance>> BuildNewInstanceMap(EndpointInstance instanceName)
    {
        var newInstanceMap = new Dictionary<string, HashSet<EndpointInstance>>();
        foreach (var pair in instanceMap)
        {
            var otherInstances = pair.Value.Where(x => x != instanceName);
            newInstanceMap[pair.Key] = new HashSet<EndpointInstance>(otherInstances);
        }
        HashSet<EndpointInstance> endpointEntry;
        var endpointName = instanceName.Endpoint;
        if (!newInstanceMap.TryGetValue(endpointName, out endpointEntry))
        {
            endpointEntry = new HashSet<EndpointInstance>();
            newInstanceMap[endpointName] = endpointEntry;
        }
        endpointEntry.Add(instanceName);
        return newInstanceMap;
    }

    static Dictionary<Type, HashSet<string>> BuildNewEndpointMap(string endpointName, Type[] types, Dictionary<Type, HashSet<string>> endpointMap)
    {
        var newEndpointMap = new Dictionary<Type, HashSet<string>>();
        foreach (var pair in endpointMap)
        {
            var otherEndpoints = pair.Value.Where(x => x != endpointName);
            newEndpointMap[pair.Key] = new HashSet<string>(otherEndpoints);
        }

        foreach (var type in types)
        {
            HashSet<string> typeEntry;
            if (!newEndpointMap.TryGetValue(type, out typeEntry))
            {
                typeEntry = new HashSet<string>();
                newEndpointMap[type] = typeEntry;
            }
            typeEntry.Add(endpointName);
        }
        return newEndpointMap;
    }

    #region FindPublisher
    PublisherAddress FindPublisher(Type eventType)
    {
        string publisherEndpoint;
        if (!publisherMap.TryGetValue(eventType, out publisherEndpoint))
        {
            return null;
        }
        return PublisherAddress.CreateFromEndpointName(publisherEndpoint);
    }
    #endregion

    #region FindEndpoint
    IEnumerable<IUnicastRoute> FindEndpoint(Type[] enclosedMessageTypes)
    {
        foreach (var type in enclosedMessageTypes)
        {
            HashSet<string> destinations;
            if (!endpointMap.TryGetValue(type, out destinations))
            {
                continue;
            }
            foreach (var destination in destinations)
            {
                yield return UnicastRoute.CreateFromEndpointName(destination);
            }
        }
    }
    #endregion

    #region FindInstance
    Task<IEnumerable<EndpointInstance>> FindInstancesTask(string endpointName)
    {
        return Task.FromResult(FindInstances(endpointName));
    }

    IEnumerable<EndpointInstance> FindInstances(string endpointName)
    {
        HashSet<EndpointInstance> instances;
        if (!instanceMap.TryGetValue(endpointName, out instances))
        {
            return Enumerable.Empty<EndpointInstance>();
        }
        var activeInstances = instances.Where(i => instanceInformation[i].State == InstanceState.Active)
            .ToArray();
        if (activeInstances.Any())
        {
            return activeInstances;
        }
        log.Info($"No active instances of endpoint {endpointName} detected. Trying to route to the inactive ones.");
        return instances;
    }
    #endregion
}
