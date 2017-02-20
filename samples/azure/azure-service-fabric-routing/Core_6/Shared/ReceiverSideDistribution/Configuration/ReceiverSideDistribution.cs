using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Transport;

namespace Shared
{
    class ReceiverSideDistribution : Feature
    {
        internal const string Discriminators = "ReceiverSideDistribution.Discriminators";
        internal const string Mapper = "ReceiverSideDistribution.Mapper";
        internal const string Logger = "ReceiverSideDistribution.Logger";

        public ReceiverSideDistribution()
        {
            Defaults(s => s.AddUnrecoverableException(typeof(PartitionMappingFailedException)));
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
            var discriminators = context.Settings.Get<HashSet<string>>(Discriminators);
            var mapper = context.Settings.Get<Func<object,string>>(Mapper);
            var logger = context.Settings.Get<Action<string>>(Logger);

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
            context.Pipeline.Register(new DistributeMessagesBasedOnPayload(discriminator, forwarder, mapper, logger), "Distributes on the receiver side using user supplied mapper");
            context.Pipeline.Register(new HardcodeReplyToAddressToLogicalAddress(context.Settings.InstanceSpecificQueue()), "Hardcodes the ReplyToAddress to the instance specific address of this endpoint.");
        }
    }
}