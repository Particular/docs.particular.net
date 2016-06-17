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

class RoutingInfoSubscriber : FeatureStartupTask
{
    static readonly ILog Logger = LogManager.GetLogger<RoutingInfoSubscriber>();

    UnicastRoutingTable routingTable;
    EndpointInstances endpointInstances;
    readonly IReadOnlyCollection<Type> messageTypesHandledByThisEndpoint;
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
        this.messageSession = context;

        #region AddDynamic
        routingTable.AddDynamic((list, bag) => FindEndpoint(list));
        endpointInstances.AddDynamic(FindInstances);
        publishers.AddDynamic(FindPublisher);
        #endregion

        sweepTimer = new Timer(state =>
        {
            foreach (var info in instanceInformation)
            {
                if (!info.Value.Sweep(DateTime.UtcNow, heartbeatTimeout))
                {
                    Logger.Info($"Instance {info.Key} deactivated (heartbeat timeout).");
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


    public async Task OnRemoved(Entry e)
    {
        var deserializedData = JsonConvert.DeserializeObject<RoutingInfo>(e.Data);
        var instanceName = new EndpointInstance(deserializedData.EndpointName, deserializedData.Discriminator, deserializedData.InstanceProperties);

        Logger.Info($"Instance {instanceName} removed from routing tables.");

        await UpdateCaches(instanceName, new Type[0], new Type[0]).ConfigureAwait(false);

        instanceInformation.Remove(instanceName);
    }

    public async Task OnChanged(Entry e)
    {
        var deserializedData = JsonConvert.DeserializeObject<RoutingInfo>(e.Data);
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
            Logger.Debug($"Instance {instanceName} active (heartbeat).");
        }
        else
        {
            instanceInfo.Deactivate();
            Logger.Info($"Instance {instanceName} deactivated.");
        }
        await UpdateCaches(instanceName, handledTypes, publishedTypes).ConfigureAwait(false);
    }

    async Task UpdateCaches(EndpointInstance instanceName, Type[] handledTypes, Type[] publishedTypes)
    {
        var newInstanceMap = BuildNewInstanceMap(instanceName);
        var newEndpointMap = BuildNewEndpointMap(instanceName.Endpoint.ToString(), handledTypes, endpointMap);
        var newPublisherMap = BuildNewPublisherMap(instanceName, publishedTypes, publisherMap);
        LogChangesToEndpointMap(endpointMap, newEndpointMap);
        LogChangesToInstanceMap(instanceMap, newInstanceMap);
        var toSubscribe = LogChangesToPublisherMap(publisherMap, newPublisherMap).ToArray();
        instanceMap = newInstanceMap;
        endpointMap = newEndpointMap;
        publisherMap = newPublisherMap;

        foreach (var type in toSubscribe.Intersect(messageTypesHandledByThisEndpoint))
        {
            await messageSession.Subscribe(type).ConfigureAwait(false);
        }
    }

    static IEnumerable<Type> LogChangesToPublisherMap(Dictionary<Type, string> publisherMap, Dictionary<Type, string> newPublisherMap)
    {
        foreach (var addedType in newPublisherMap.Keys.Except(publisherMap.Keys))
        {
            Logger.Info($"Added {newPublisherMap[addedType]} as publisher of {addedType}.");
            yield return addedType;
        }

        foreach (var removedType in publisherMap.Keys.Except(newPublisherMap.Keys))
        {
            Logger.Info($"Removed {publisherMap[removedType]} as publisher of {removedType}.");
        }
    }
   
    static void LogChangesToInstanceMap(Dictionary<string, HashSet<EndpointInstance>> instanceMap, Dictionary<string, HashSet<EndpointInstance>> newInstanceMap)
    {
        foreach (var addedEndpoint in newInstanceMap.Keys.Except(instanceMap.Keys))
        {
            Logger.Info($"Added endpoint {addedEndpoint} with instances {FormatSet(newInstanceMap[addedEndpoint])}");
        }

        foreach (var removedEndpoint in instanceMap.Keys.Except(newInstanceMap.Keys))
        {
            Logger.Info($"Removed endpoint {removedEndpoint} with instances {FormatSet(instanceMap[removedEndpoint])}");
        }

        foreach (var existingEndpoint in instanceMap.Keys.Intersect(newInstanceMap.Keys))
        {
            var addedInstances = newInstanceMap[existingEndpoint].Except(instanceMap[existingEndpoint]).ToArray();
            var removedInstances = instanceMap[existingEndpoint].Except(newInstanceMap[existingEndpoint]).ToArray();
            if (addedInstances.Any())
            {
                Logger.Info($"Added instances {FormatSet(addedInstances)} to endpoint {existingEndpoint}");
            }
            if (removedInstances.Any())
            {
                Logger.Info($"Removed instances {FormatSet(removedInstances)} from endpoint {existingEndpoint}");
            }
        }
    }

    static void LogChangesToEndpointMap(Dictionary<Type, HashSet<string>> endpointMap, Dictionary<Type, HashSet<string>> newEndpointMap)
    {
        foreach (var addedType in newEndpointMap.Keys.Except(endpointMap.Keys))
        {
            Logger.Info($"Added route for {addedType.Name} to {FormatSet(newEndpointMap[addedType])}");
        }

        foreach (var removedType in endpointMap.Keys.Except(newEndpointMap.Keys))
        {
            Logger.Info($"Removed route for {removedType.Name} to {FormatSet(newEndpointMap[removedType])}");
        }

        foreach (var existingType in endpointMap.Keys.Intersect(newEndpointMap.Keys))
        {
            var addedEndpoints = newEndpointMap[existingType].Except(endpointMap[existingType]).ToArray();
            var removedEndpoints = endpointMap[existingType].Except(newEndpointMap[existingType]).ToArray();
            if (addedEndpoints.Any())
            {
                Logger.Info($"Added route for {existingType.Name} to {FormatSet(addedEndpoints)}");
            }
            if (removedEndpoints.Any())
            {
                Logger.Info($"Removed route for {existingType.Name} to {FormatSet(removedEndpoints)}");
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
        foreach (var pair in publisherMap)
        {
            if (pair.Value != endpointInstance.Endpoint.ToString())
            {
                newPublisherMap[pair.Key] = pair.Value;
            }
        }
        foreach (var publishedType in publishedTypes)
        {
            if (newPublisherMap.ContainsKey(publishedType))
            {
                Logger.Warn($"More than one endpoint advertises publishing of {publishedType}: {newPublisherMap[publishedType]}, {endpointInstance.Endpoint}");
            }
            else
            {
                newPublisherMap[publishedType] = endpointInstance.Endpoint.ToString();
                //Subscribe
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
        if (!newInstanceMap.TryGetValue(instanceName.Endpoint.ToString(), out endpointEntry))
        {
            endpointEntry = new HashSet<EndpointInstance>();
            newInstanceMap[instanceName.Endpoint.ToString()] = endpointEntry;
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
        return publisherMap.TryGetValue(eventType, out publisherEndpoint) 
            ? new PublisherAddress(new EndpointName(publisherEndpoint)) 
            : null;
    }
    #endregion

    #region FindEndpoint
    IEnumerable<IUnicastRoute> FindEndpoint(List<Type> enclosedMessageTypes)
    {
        foreach (var type in enclosedMessageTypes)
        {
            HashSet<string> destinations;
            if (endpointMap.TryGetValue(type, out destinations))
            {
                foreach (var endpointName in destinations)
                {
                    yield return new UnicastRoute(new EndpointName(endpointName));
                }
            }
        }
    }
    #endregion

    #region FindInstance
    Task<IEnumerable<EndpointInstance>> FindInstances(EndpointName endpointName)
    {
        HashSet<EndpointInstance> instances;
        if (instanceMap.TryGetValue(endpointName.ToString(), out instances))
        {
            var activeInstances = instances.Where(i => instanceInformation[i].State == InstanceState.Active).ToArray();
            if (activeInstances.Any())
            {
                return Task.FromResult((IEnumerable<EndpointInstance>)activeInstances);
            }
            Logger.Info($"No active instances of endpoint {endpointName} detected. Trying to route to the inactive ones.");
            return Task.FromResult((IEnumerable<EndpointInstance>)instances);
        }
        return Task.FromResult(Enumerable.Empty<EndpointInstance>());
    }
    #endregion
}
