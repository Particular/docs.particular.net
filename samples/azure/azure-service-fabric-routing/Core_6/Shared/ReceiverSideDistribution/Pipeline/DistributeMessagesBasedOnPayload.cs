using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Pipeline;

namespace Shared
{
    class DistributeMessagesBasedOnPayload : IBehavior<IIncomingLogicalMessageContext, IIncomingLogicalMessageContext>
    {
        readonly string localPartitionKey;
        readonly Forwarder forwarder;
        readonly Func<object, string> mapper;
        readonly Action<string> logger;

        public DistributeMessagesBasedOnPayload(string localPartitionKey, Forwarder forwarder, Func<object, string> mapper, Action<string> logger)
        {
            this.localPartitionKey = localPartitionKey;
            this.forwarder = forwarder;
            this.mapper = mapper;
            this.logger = logger;
        }

        public Task Invoke(IIncomingLogicalMessageContext context, Func<IIncomingLogicalMessageContext, Task> next)
        {
            string messagePartitionKey;

            var intent = GetMessageIntent(context);
            var isSubscriptionMessage = intent == MessageIntentEnum.Subscribe || intent == MessageIntentEnum.Unsubscribe;
            var isReply = intent == MessageIntentEnum.Reply;

            if (isSubscriptionMessage || isReply)
            {
                return next(context);
            }

            if (!context.MessageHeaders.TryGetValue(PartitionHeaders.PartitionKey, out messagePartitionKey))
            {
                messagePartitionKey = mapper(context.Message.Instance);

                if (string.IsNullOrWhiteSpace(messagePartitionKey))
                {
                    throw new PartitionMappingFailedException($"Could not map a partition key for message of type {context.Headers[Headers.EnclosedMessageTypes]}");
                }
            }

            var message = $"##### Received message: {context.Headers[Headers.EnclosedMessageTypes]} with Mapped PartitionKey={messagePartitionKey} on partition {localPartitionKey}";

            logger(message);

            if (messagePartitionKey == localPartitionKey)
            {
                return next(context);
            }

            context.Headers[PartitionHeaders.PartitionKey] = messagePartitionKey;

            return forwarder.Forward(context, messagePartitionKey);
        }

        static MessageIntentEnum? GetMessageIntent(IMessageProcessingContext context)
        {
            string intentStr;

            if (context.MessageHeaders.TryGetValue(Headers.MessageIntent, out intentStr))
            {
                MessageIntentEnum intent;
                if (Enum.TryParse(intentStr, out intent))
                {
                    return intent;
                }
                return null;
            }

            return null;
        }
    }
}