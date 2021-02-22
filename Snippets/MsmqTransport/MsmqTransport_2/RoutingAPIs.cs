// ReSharper disable UnusedParameter.Local

namespace Core6.Routing
{
    using System.Collections.Generic;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.Routing;

    class RoutingAPIs
    {
        void MapMessagesToLogicalEndpoints(EndpointConfiguration endpointConfiguration)
        {
            #region Routing-MapMessagesToLogicalEndpoints

            var routing = endpointConfiguration.UseTransport(new MsmqTransport());

            routing.RouteToEndpoint(
                messageType: typeof(AcceptOrder),
                destination: "Sales");
            routing.RouteToEndpoint(
                messageType: typeof(SendOrder),
                destination: "Shipping");

            #endregion
        }

        class Instances :
            Feature
        {
            #region RoutingExtensibility-Instances
            protected override void Setup(FeatureConfigurationContext context)
            {
                var endpointInstances = context.Settings.Get<EndpointInstances>();
                endpointInstances.AddOrReplaceInstances("MySource",
                    new List<EndpointInstance>
                    {
                        new EndpointInstance("MyEndpoint").AtMachine("VM-1"),
                        new EndpointInstance("MyEndpoint").AtMachine("VM-2")
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