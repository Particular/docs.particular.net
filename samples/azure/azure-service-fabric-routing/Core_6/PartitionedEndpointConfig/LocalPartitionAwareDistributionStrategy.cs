namespace PartitionedEndpointConfig
{
    using System;
    using System.Linq;
    using NServiceBus;
    using NServiceBus.Routing;

    internal class LocalPartitionAwareDistributionStrategy : DistributionStrategy
    {
        readonly string localDiscriminator;

        public LocalPartitionAwareDistributionStrategy(string endpoint, string localDiscriminator) : base(endpoint, DistributionStrategyScope.Send)
        {
            this.localDiscriminator = localDiscriminator;
        }

        public override string SelectReceiver(string[] receiverAddresses)
        {
            throw new NotImplementedException();
        }

        public override string SelectDestination(DistributionContext context)
        {
            context.Headers[PartitionHeaders.PartitionKey] = localDiscriminator;

            var logicalAddress = LogicalAddress.CreateRemoteAddress(new EndpointInstance(Endpoint, localDiscriminator));
            return context.ReceiverAddresses.Single(a => a == logicalAddress.ToString());
        }
    }
}
