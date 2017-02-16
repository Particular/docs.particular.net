using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Pipeline;

namespace Contracts
{
    class DistributeMessagesBasedOnPayload : IBehavior<IIncomingLogicalMessageContext, IIncomingLogicalMessageContext>
    {
        private readonly string localPartitionKey;
        private readonly Forwarder forwarder;
        private readonly Func<object, string> mapper;
        private readonly Action<string> logger;

        public DistributeMessagesBasedOnPayload(string localPartitionKey, Forwarder forwarder, Func<object, string> mapper, Action<string> logger)
        {
            this.localPartitionKey = localPartitionKey;
            this.forwarder = forwarder;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task Invoke(IIncomingLogicalMessageContext context, Func<IIncomingLogicalMessageContext, Task> next)
        {
            string messagePartitionKey;

            if (!context.MessageHeaders.TryGetValue(PartitionHeaders.PartitionKey, out messagePartitionKey))
            {
                messagePartitionKey = mapper(context.Message.Instance);

                if (string.IsNullOrWhiteSpace(messagePartitionKey))
                {
                    throw new Exception($"Could not map a partition key for message of type {context.Headers[Headers.EnclosedMessageTypes]}"); //Will be replaced by unrecoverable exception part of Core PR 4479
                }
            }

            var message = $"##### Received message: {context.Headers[Headers.EnclosedMessageTypes]} with Mapped PartitionKey={messagePartitionKey} on partition {localPartitionKey}";

            logger(message);

            if (messagePartitionKey == localPartitionKey)
            {
                await next(context).ConfigureAwait(false); //Continue the pipeline as usual
                return;
            }

            context.Headers[PartitionHeaders.PartitionKey] = messagePartitionKey;

            await forwarder.Forward(context, messagePartitionKey).ConfigureAwait(false);
        }
    }
}