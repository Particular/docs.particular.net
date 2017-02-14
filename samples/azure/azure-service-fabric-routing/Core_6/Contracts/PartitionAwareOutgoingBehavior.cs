using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

namespace Contracts
{
    public abstract class PartitionAwareOutgoingBehavior : IBehavior<IOutgoingLogicalMessageContext, IOutgoingLogicalMessageContext>
    {
        public Task Invoke(IOutgoingLogicalMessageContext context, Func<IOutgoingLogicalMessageContext, Task> next)
        {
            var discriminator = MapMessageToPartition(context.Message.Instance);

            // stamp message with the partition key so that behavior used for receiver side can identify the message destination
            context.Headers[PartitionHeaders.PartitionKey] = discriminator;

            return next(context);
        }

        protected abstract string MapMessageToPartition(object message);
    }
}