// ReSharper disable UnusedParameter.Local

namespace Core6.Routing
{
    using System.Collections.Generic;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.Routing.MessageDrivenSubscriptions;

    class RoutingAPIs
    {
        void MapMessagesToLogicalEndpoints(EndpointConfiguration endpointConfiguration)
        {
            #region Routing-MapMessagesToLogicalEndpoints

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            var routing = transport.Routing();

            routing.RouteToEndpoint(
                messageType: typeof(AcceptOrder),
                destination: "Sales");
            routing.RouteToEndpoint(
                messageType: typeof(SendOrder),
                destination: "Shipping");

            #endregion
        }


        class PublishersTable :
            Feature
        {
            #region RoutingExtensibility-Publishers
            protected override void Setup(FeatureConfigurationContext context)
            {
                var publishers = context.Settings.Get<Publishers>();
                var publisherAddress = PublisherAddress.CreateFromEndpointName("PublisherEndpoint");
                publishers.AddOrReplacePublishers("MySource",
                    new List<PublisherTableEntry>
                    {
                        new PublisherTableEntry(typeof(MyEvent), publisherAddress)
                    });
            }
            #endregion
        }

        class AcceptOrder
        {
        }

        class SendOrder
        {
        }
        
        class MyEvent :
            IEvent
        {
        }
    }
}