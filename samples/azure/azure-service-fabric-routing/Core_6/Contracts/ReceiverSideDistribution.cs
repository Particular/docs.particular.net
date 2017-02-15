using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Transport;

namespace Contracts
{
    class ReceiverSideDistribution : Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
            var discriminators = context.Settings.Get<HashSet<string>>("ReceiverSideDistribution.Discriminators");
            var mapper = context.Settings.Get<Func<object,string>>("ReceiverSideDistribution.Mapper");
            Action<string> logger = context.Settings.Get<Action<string>>("ReceiverSideDistribution.Logger");

            var discriminator = context.Settings.Get<string>("EndpointInstanceDiscriminator");
            var transportInfrastructure = context.Settings.Get<TransportInfrastructure>();
            var logicalAddress = context.Settings.LogicalAddress();

            var supportMessageDrivenPubSub = context.Settings.Get<TransportInfrastructure>().OutboundRoutingPolicy.Publishes == OutboundRoutingType.Unicast;

            if (supportMessageDrivenPubSub)
            {
                context.Pipeline.Register(new SubscriptionDistributionBehavior.Register(discriminator, discriminators, address => transportInfrastructure.ToTransportAddress(address), logicalAddress));
            }

            var forwarder = new Forwarder(discriminators, address => transportInfrastructure.ToTransportAddress(address), logicalAddress);
            context.Pipeline.Register(new DistributeMessagesBasedOnHeader(discriminator, forwarder), "Distributes on the receiver side using header only");
            context.Pipeline.Register(new DistributeMessagesBasedOnPayload(discriminator, forwarder, mapper, logger), "Distributes on the receiver side using user supplied mapper");
        }
    }
}