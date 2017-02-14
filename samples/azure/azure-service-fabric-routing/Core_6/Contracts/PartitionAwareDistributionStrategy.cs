namespace Contracts
{
    using System;
    using System.Linq;
    using NServiceBus;
    using NServiceBus.Routing;

    public abstract class PartitionAwareDistributionStrategy : DistributionStrategy
    {
        protected PartitionAwareDistributionStrategy(string endpoint, DistributionStrategyScope scope) : base(endpoint, scope)
        {
        }

        public override string SelectReceiver(string[] receiverAddresses)
        {
            throw new NotImplementedException();
        }

        public override string SelectDestination(DistributionContext context)
        {
            var discriminator = MapMessageToPartition(context.Message.Instance);

            context.Headers[PartitionHeaders.PartitionKey] = discriminator;

            var logicalAddress = LogicalAddress.CreateRemoteAddress(new EndpointInstance(Endpoint, discriminator));
            return context.ReceiverAddresses.Single(a => a == logicalAddress.ToString());
        }

        protected abstract string MapMessageToPartition(object message);
    }
}
