using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Pipeline;

namespace Shared
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

            var intent = default(MessageIntentEnum);
            string str;
            if (context.MessageHeaders.TryGetValue("NServiceBus.MessageIntent", out str))
            {
                Enum.TryParse(str, true, out intent);
            }
            var isSubscriptionMessage = intent == MessageIntentEnum.Subscribe || intent == MessageIntentEnum.Unsubscribe;
            var isReply = intent == MessageIntentEnum.Reply;

            if (isSubscriptionMessage || isReply)
            {
                await next(context).ConfigureAwait(false);
                return;
            }

            if (!context.MessageHeaders.TryGetValue(PartitionHeaders.PartitionKey, out messagePartitionKey))
            {
                messagePartitionKey = mapper(context.Message.Instance);

                if (string.IsNullOrWhiteSpace(messagePartitionKey))
                {
                    if (IsReply(context))
                    {
                        await next(context).ConfigureAwait(false);
                        return;
                    }

                    throw new PartitionMappingFailedException($"Could not map a partition key for message of type {context.Headers[Headers.EnclosedMessageTypes]}");
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

        static bool IsReply(IMessageProcessingContext context)
        {
            string intentStr;

            if (context.MessageHeaders.TryGetValue(Headers.MessageIntent, out intentStr))
            {
                MessageIntentEnum intent;
                if (Enum.TryParse(intentStr, out intent))
                {
                    return intent == MessageIntentEnum.Reply;
                }
                return false;
            }

            return false;
        }
    }
}