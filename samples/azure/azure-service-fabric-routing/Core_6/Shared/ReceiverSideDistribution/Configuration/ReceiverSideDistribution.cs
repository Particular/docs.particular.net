using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Transport;

namespace Shared
{
    class ReceiverSideDistribution : Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
            var options = context.Settings.Get<ReceiverSideDistributionOptions>("ReceiverSideDistribution.Options");
            var discriminators = context.Settings.Get<HashSet<string>>("ReceiverSideDistribution.Discriminators");
            var mapper = context.Settings.Get<Func<object,string>>("ReceiverSideDistribution.Mapper");
            Action<string> logger = context.Settings.Get<Action<string>>("ReceiverSideDistribution.Logger");

            var discriminator = context.Settings.Get<string>("EndpointInstanceDiscriminator");
            var transportInfrastructure = context.Settings.Get<TransportInfrastructure>();
            var logicalAddress = context.Settings.LogicalAddress();

            var supportMessageDrivenPubSub = context.Settings.Get<TransportInfrastructure>().OutboundRoutingPolicy.Publishes == OutboundRoutingType.Unicast;

            if (supportMessageDrivenPubSub)
            {
                context.Pipeline.Register(new DistributeSubscriptions.Register(discriminator, discriminators, address => transportInfrastructure.ToTransportAddress(address), logicalAddress));
            }

            var forwarder = new Forwarder(discriminators, address => transportInfrastructure.ToTransportAddress(address), logicalAddress);
            context.Pipeline.Register(new DistributeMessagesBasedOnHeader(discriminator, forwarder, logger), "Distributes on the receiver side using header only");
            context.Pipeline.Register(new DistributeMessagesBasedOnPayload(discriminator, forwarder, mapper, options.TrustReplies, logger), "Distributes on the receiver side using user supplied mapper");
        }
    }
}