using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Transport;

namespace Shared
{
    class DistributeMessagesBasedOnHeader : IBehavior<IIncomingPhysicalMessageContext, IIncomingPhysicalMessageContext>
    {
        private readonly string localPartitionKey;
        private readonly Forwarder forwarder;
        readonly Action<string> logger;

        public DistributeMessagesBasedOnHeader(string localPartitionKey, Forwarder forwarder, Action<string> logger)
        {
            this.localPartitionKey = localPartitionKey;
            this.forwarder = forwarder;
            this.logger = logger;
        }

        public async Task Invoke(IIncomingPhysicalMessageContext context, Func<IIncomingPhysicalMessageContext, Task> next)
        {
            var intent = context.Message.GetMesssageIntent();
            var isSubscriptionMessage = intent == MessageIntentEnum.Subscribe || intent == MessageIntentEnum.Unsubscribe;

            if (isSubscriptionMessage)
            {
                await next(context).ConfigureAwait(false);
            }

            string messagePartitionKey;
            var hasPartitionKeyHeader = context.MessageHeaders.TryGetValue(PartitionHeaders.PartitionKey, out messagePartitionKey);

            //1. The header value isn't present (logical behavior will check message contents)
            //2. The header value matches local partition key
            if (!hasPartitionKeyHeader || messagePartitionKey == localPartitionKey)
            {
                await next(context).ConfigureAwait(false); //Continue the pipeline as usual
                return;
            }
            context.Message.Headers[PartitionHeaders.PartitionKey] = messagePartitionKey;

            var message = $"##### Received message: {context.Message.Headers[Headers.EnclosedMessageTypes]} with Mapped PartitionKey={messagePartitionKey} on partition {localPartitionKey}";

            logger(message);

            await forwarder.Forward(context, messagePartitionKey).ConfigureAwait(false);
        }
    }
}