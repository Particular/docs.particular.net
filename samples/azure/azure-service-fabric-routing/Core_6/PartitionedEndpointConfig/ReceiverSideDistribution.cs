namespace PartitionedEndpointConfig
{
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.Transport;

    class ReceiverSideDistribution : Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
            var configuration = context.Settings.Get<ReceiverSideDistributionConfiguration>();

            var discriminator = context.Settings.Get<string>("EndpointInstanceDiscriminator");
            var transportInfrastructure = context.Settings.Get<TransportInfrastructure>();
            var logicalAddress = context.Settings.LogicalAddress();

            var supportMessageDrivenPubSub = context.Settings.Get<TransportInfrastructure>().OutboundRoutingPolicy.Publishes == OutboundRoutingType.Unicast;

            if (supportMessageDrivenPubSub)
            {
                context.Pipeline.Register(new SubscriptionDistributionBehavior.Register(discriminator, configuration.Discriminators, address => transportInfrastructure.ToTransportAddress(address), logicalAddress));
            }

            var forwarder = new Forwarder(configuration.Discriminators, address => transportInfrastructure.ToTransportAddress(address), logicalAddress);
            context.Pipeline.Register(new DistributeMessagesBasedOnHeader(discriminator, forwarder, configuration.Logger), "Distributes on the receiver side using header only");
            context.Pipeline.Register(new DistributeMessagesBasedOnPayload(discriminator, forwarder, configuration.MapMessageToPartitionKey, configuration.TrustReplies, configuration.Logger), "Distributes on the receiver side using user supplied mapper");
        }
    }
}