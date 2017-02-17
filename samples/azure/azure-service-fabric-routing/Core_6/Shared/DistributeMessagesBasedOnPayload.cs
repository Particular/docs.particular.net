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
        private readonly bool trustReplies;
        private readonly Action<string> logger;

        public DistributeMessagesBasedOnPayload(string localPartitionKey, Forwarder forwarder, Func<object, string> mapper, bool trustReplies, Action<string> logger)
        {
            this.localPartitionKey = localPartitionKey;
            this.forwarder = forwarder;
            this.mapper = mapper;
            this.trustReplies = trustReplies;
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
                    if (trustReplies && IsReply(context))
                    {
                        await next(context).ConfigureAwait(false);
                        return;
                    }

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

        static bool IsReply(IIncomingLogicalMessageContext context)
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