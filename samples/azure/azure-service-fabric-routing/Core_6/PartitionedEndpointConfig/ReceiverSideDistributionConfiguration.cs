using System;
using System.Collections.Generic;

namespace PartitionedEndpointConfig
{
    public class ReceiverSideDistributionConfiguration
    {
        internal HashSet<string> Discriminators { get; }
        internal Action<string> Logger { get; }
        internal bool TrustReplies { get; }

        private readonly Dictionary<Type, Func<object, string>> messageTypeMappers = new Dictionary<Type, Func<object, string>>();

        internal ReceiverSideDistributionConfiguration(HashSet<string> discriminators, Action<string> logger, bool trustReplies)
        {
            Discriminators = discriminators;
            Logger = logger;
            TrustReplies = trustReplies;
        }

        public void AddMappingForMessageType<TMessage>(Func<TMessage, string> mapMessageToPartionKey)
        {
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