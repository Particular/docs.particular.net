using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Transport;

namespace PartionAwareSenderSideDistribution
{
    public class PartitionAwareSenderSideDistributionConfiguration<TTransport> where TTransport : TransportDefinition
    {
        private readonly string destinationEndpoint;
        private readonly TransportExtensions<TTransport> transportConfig;
        private readonly Dictionary<Type, Func<object,string>> messageTypeMappers = new Dictionary<Type, Func<object, string>>();

        internal PartitionAwareSenderSideDistributionConfiguration(string destinationEndpoint, TransportExtensions<TTransport> transportConfig)
        {
            this.destinationEndpoint = destinationEndpoint;
            this.transportConfig = transportConfig;
        }

        public void AddMappingForMessageType<TMessage>(Func<TMessage, string> mapMessageToPartionKey)
        {
            transportConfig.Routing().RouteToEndpoint(typeof(TMessage), destinationEndpoint);
            messageTypeMappers[typeof(TMessage)] = message => mapMessageToPartionKey((TMessage)message);
        }

        internal string MapMessageToPartitionKey(object message)
        {
            var messageType = message.GetType();

            if (!messageTypeMappers.ContainsKey(messageType))
            {
                throw new Exception($"No partition mapping is found for message type '{messageType}'.");
            }

            var mapper = messageTypeMappers[messageType];

            return mapper(message);
        }
    }
}