using System;
using System.Linq;
using NServiceBus;
using NServiceBus.Routing;

namespace Messages
{
    public class PartitionAwareDistributionStrategy : DistributionStrategy
    {
        public PartitionAwareDistributionStrategy(string endpoint, DistributionStrategyScope scope) : base(endpoint, scope)
        {
        }

        public override string SelectReceiver(string[] receiverAddresses)
        {
            throw new NotImplementedException();
        }

        public override string SelectDestination(DistributionContext context)
        {
            var discriminator = PartitionMap.Map(context.Message.Instance);
            var logicalAddress = LogicalAddress.CreateRemoteAddress(new EndpointInstance(Endpoint, discriminator));
            return context.ReceiverAddresses.Single(a => a == logicalAddress.ToString());
        }
    }
}
