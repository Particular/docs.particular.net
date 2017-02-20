using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Transport;

namespace Shared
{
    class ReceiverSideDistribution : Feature
    {
        public ReceiverSideDistribution()
        {
            Defaults(s => s.AddUnrecoverableException(typeof(PartitionMappingFailedException)));
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
            var configuration = context.Settings.Get<PartitionAwareReceiverSideDistributionConfiguration>();

            var discriminator = context.Settings.Get<string>("EndpointInstanceDiscriminator");
            var transportInfrastructure = context.Settings.Get<TransportInfrastructure>();
            var logicalAddress = context.Settings.LogicalAddress();

            var supportMessageDrivenPubSub = context.Settings.Get<TransportInfrastructure>().OutboundRoutingPolicy.Publishes == OutboundRoutingType.Unicast;

            if (supportMessageDrivenPubSub)
            {
                context.Pipeline.Register(new DistributeSubscriptions.Register(discriminator, configuration.Partitions, address => transportInfrastructure.ToTransportAddress(address), logicalAddress));
            }

            var forwarder = new Forwarder(configuration.Partitions, address => transportInfrastructure.ToTransportAddress(address), logicalAddress);
            context.Pipeline.Register(new DistributeMessagesBasedOnHeader(discriminator, forwarder, configuration.Logger), "Distributes on the receiver side using header only");
            context.Pipeline.Register(new DistributeMessagesBasedOnPayload(discriminator, forwarder, configuration.MapMessageToPartitionKey, configuration.Logger), "Distributes on the receiver side using user supplied mapper");
            context.Pipeline.Register(new HardcodeReplyToAddressToLogicalAddress(context.Settings.InstanceSpecificQueue()), "Hardcodes the ReplyToAddress to the instance specific address of this endpoint.");
        }
    }
}