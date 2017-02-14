namespace Contracts
{
    using System;
    using System.Linq;
    using NServiceBus;
    using NServiceBus.Routing;

    public abstract class PartitionAwareDistributionStrategy : DistributionStrategy
    {
        public PartitionAwareDistributionStrategy(string endpoint, DistributionStrategyScope scope) : base(endpoint, scope)
        {
        }

        public override string SelectReceiver(string[] receiverAddresses)
        {
            throw new NotImplementedException();
        }

        public abstract string MapMessageToPartition(object message);

        public override string SelectDestination(DistributionContext context)
        {
            var discriminator = MapMessageToPartition(context.Message.Instance);

            // stampt message with the partition key so that behavior used for receiver side can identify the message destination
            context.Headers["partition-key"] = discriminator;

            var logicalAddress = LogicalAddress.CreateRemoteAddress(new EndpointInstance(Endpoint, discriminator));
            return context.ReceiverAddresses.Single(a => a == logicalAddress.ToString());
        }
    }
}
