using System;
using System.Collections.Generic;
using System.Linq;
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
    Dictionary<Type, string> endpointMap = new Dictionary<Type, string>();
    Dictionary<string, HashSet<EndpointInstance>> instanceMap = new Dictionary<string, HashSet<EndpointInstance>>();
    Dictionary<Type, string> publisherMap = new Dictionary<Type, string>();
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
        return Task.CompletedTask;
    }

    protected override Task OnStop(IMessageSession session)
    {
        return Task.CompletedTask;
    }

    public async Task OnRemoved(Entry entry)
    {
        var deserializedData = JsonConvert.DeserializeObject<RoutingInfo>(entry.Data);
        var instanceName = new EndpointInstance(deserializedData.EndpointName, deserializedData.Discriminator, deserializedData.InstanceProperties);

        log.Info($"Instance {instanceName} removed from routing tables.");

        await UpdateCaches(instanceName, new Type[0], new Type[0])
            .ConfigureAwait(false);
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

        #region AddOrReplace

        routingTable.AddOrReplaceRoutes("AutomaticRouting", newEndpointMap.Select(
            x => new RouteTableEntry(x.Key, UnicastRoute.CreateFromEndpointName(x.Value))).ToList());

        publishers.AddOrReplacePublishers("AutomaticRouting", newPublisherMap.Select(
            x => new PublisherTableEntry(x.Key, PublisherAddress.CreateFromEndpointName(x.Value))).ToList());

        endpointInstances.AddOrReplaceInstances("AutomaticRouting", newInstanceMap.SelectMany(x => x.Value).ToList());

        #endregion

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

    static void LogChangesToEndpointMap(Dictionary<Type, string> endpointMap, Dictionary<Type, string> newEndpointMap)
    {
        foreach (var addedType in newEndpointMap.Keys.Except(endpointMap.Keys))
        {
            log.Info($"Added route for {addedType.Name} to [{newEndpointMap[addedType]}]");
        }

        foreach (var removedType in endpointMap.Keys.Except(newEndpointMap.Keys))
        {
            log.Info($"Removed route for {removedType.Name} to [{newEndpointMap[removedType]}]");
        }

        foreach (var existingType in endpointMap.Keys.Intersect(newEndpointMap.Keys))
        {
            var newSet = newEndpointMap[existingType];
            var currentSet = endpointMap[existingType];
            if (newSet != currentSet)
            {
                log.Info($"Changed route for {existingType.Name} from {currentSet} to [{newSet}]");
            }
        }
    }

    static string FormatSet(IEnumerable<object> set)
    {
        return string.Join(", ", set.Select(o => $"[{o.ToString()}]"));
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
        var endpointName = instanceName.Endpoint;
        if (!newInstanceMap.TryGetValue(endpointName, out var endpointEntry))
        {
            endpointEntry = new HashSet<EndpointInstance>();
            newInstanceMap[endpointName] = endpointEntry;
        }
        endpointEntry.Add(instanceName);
        return newInstanceMap;
    }

    static Dictionary<Type, string> BuildNewEndpointMap(string endpointName, Type[] types, Dictionary<Type, string> endpointMap)
    {
        var newEndpointMap = new Dictionary<Type, string>(endpointMap);

        foreach (var type in types)
        {
            newEndpointMap[type] = endpointName;
        }
        return newEndpointMap;
    }
}