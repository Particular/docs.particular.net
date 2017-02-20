using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Settings;

namespace Shared
{
    public class PartitionAwareSenderSideDistributionConfiguration : ExposeSettings
    {
        private readonly RoutingSettings _routingSettings;
        private readonly string _endpointName;
        private readonly string[] _partitions;
        private readonly Dictionary<Type, Func<object, string>> _messageTypeMappers = new Dictionary<Type, Func<object, string>>();

        public PartitionAwareSenderSideDistributionConfiguration(SettingsHolder settings, RoutingSettings routingSettings, string endpointName, string[] partitions)
            : base(settings)
        {
            _routingSettings = routingSettings;
            _endpointName = endpointName;
            _partitions = partitions;
        }

        public PartitionAwareSenderSideDistributionConfiguration AddPartitionMappingForMessageType<T>(Func<T, string> mapMessageToPartitionKey)
        {
            _routingSettings.RouteToEndpoint(typeof(T), _endpointName);
            _messageTypeMappers[typeof(T)] = message => mapMessageToPartitionKey((T)message);

            return this;
        }

        internal string MapMessageToPartitionKey(object message)
        {
            var messageType = message.GetType();

            if (!_messageTypeMappers.ContainsKey(messageType))
            {
                throw new Exception($"No partition mapping is found for message type '{messageType}'.");
            }

            var mapper = _messageTypeMappers[messageType];

            var partition = mapper(message);

            if (!_partitions.Contains(partition))
            {
                throw new Exception($"Partition '{partition}' returned by partition mapping of '{messageType}' did not match any of the registered destination partitions '{string.Join(",", _partitions)}'.");
            }

            return partition;
        }
    }
}