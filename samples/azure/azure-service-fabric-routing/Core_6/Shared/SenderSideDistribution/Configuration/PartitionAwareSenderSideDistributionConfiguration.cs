using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;

public class PartitionAwareSenderSideDistributionConfiguration : ExposeSettings
{
    readonly RoutingSettings routingSettings;
    readonly string endpointName;
    readonly string[] partitions;
    readonly Dictionary<Type, Func<object, string>> messageTypeMappers = new Dictionary<Type, Func<object, string>>();

    public PartitionAwareSenderSideDistributionConfiguration(RoutingSettings routingSettings, string endpointName, string[] partitions)
        : base(routingSettings.GetSettings())
    {
        this.routingSettings = routingSettings;
        this.endpointName = endpointName;
        this.partitions = partitions;
    }

    public PartitionAwareSenderSideDistributionConfiguration AddPartitionMappingForMessageType<T>(Func<T, string> mapMessageToPartitionKey)
    {
        routingSettings.RouteToEndpoint(typeof(T), endpointName);
        messageTypeMappers[typeof(T)] = message => mapMessageToPartitionKey((T)message);

        return this;
    }

    internal string MapMessageToPartitionKey(object message)
    {
        var messageType = message.GetType();

        if (!messageTypeMappers.ContainsKey(messageType))
        {
            throw new Exception($"No partition mapping is found for message type '{messageType}'.");
        }

        var mapper = messageTypeMappers[messageType];

        var partition = mapper(message);

        if (!partitions.Contains(partition))
        {
            throw new Exception($"Partition '{partition}' returned by partition mapping of '{messageType}' did not match any of the registered destination partitions '{string.Join(",", partitions)}'.");
        }

        return partition;
    }
}