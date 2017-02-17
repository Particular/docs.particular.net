using System;
using System.Linq;
using NServiceBus;
using NServiceBus.Routing;

namespace PartionAwareSenderSideDistribution
{
    public class PartitionAwareDistributionStrategy : DistributionStrategy
    {
        readonly string localDiscriminator;
        private readonly Func<object, string> mapper;

        public PartitionAwareDistributionStrategy(string endpoint, Func<object, string> mapper, DistributionStrategyScope scope, string localDiscriminator = null) : base(endpoint, scope)
        {
            this.localDiscriminator = localDiscriminator;
            this.mapper = mapper;
        }

        public override string SelectReceiver(string[] receiverAddresses)
        {
            throw new NotImplementedException();
        }

        public override string SelectDestination(DistributionContext context)
        {
            var discriminator = mapper(context.Message.Instance);

            context.Headers[PartitionHeaders.PartitionKey] = discriminator;

            if (localDiscriminator != null)
            {
                context.Headers[PartitionHeaders.OriginatorPartitionKey] = localDiscriminator;
            }

            var logicalAddress = LogicalAddress.CreateRemoteAddress(new EndpointInstance(Endpoint, discriminator));
            return context.ReceiverAddresses.Single(a => a == logicalAddress.ToString());
        }
    }
}
