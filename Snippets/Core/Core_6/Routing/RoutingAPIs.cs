// ReSharper disable UnusedParameter.Local

namespace Core6.Routing
{
    using NServiceBus;

    class RoutingAPIs
    {
        void LogicalRouting(TransportExtensions transportExtensions)
        {
            #region Routing-Logical

            var routing = transportExtensions.Routing();
            routing.RouteToEndpoint(typeof(AcceptOrder).Assembly, "Sales");

            routing.RouteToEndpoint(typeof(AcceptOrder).Assembly, "PriorityMessages", "Preferred");

            routing.RouteToEndpoint(typeof(SendOrder), "Sending");

            #endregion
        }

        void SubscribeRouting(TransportExtensions<MsmqTransport> transportExtensions)
        {
            #region Routing-RegisterPublisher

            var routing = transportExtensions.Routing();
            routing.RegisterPublisher(typeof(OrderAccepted).Assembly, "Sales");

            routing.RegisterPublisher(typeof(OrderAccepted).Assembly, "PriorityMessages", "Preferred");

            routing.RegisterPublisher(typeof(OrderSent), "Sending");
            #endregion
        }

        void StaticRoutesEndpointBroker(TransportExtensions transportExtensions)
        {
            #region Routing-StaticRoutes-Endpoint-Broker

            var routing = transportExtensions.Routing();
            routing.RouteToEndpoint(typeof(AcceptOrder), "Sales");

            #endregion
        }

        void MapMessagesToLogicalEndpoints(EndpointConfiguration endpointConfiguration)
        {
            #region Routing-MapMessagesToLogicalEndpoints

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            var routing = transport.Routing();

            routing.RouteToEndpoint(typeof(AcceptOrder), "Sales");
            routing.RouteToEndpoint(typeof(SendOrder), "Shipping");

            #endregion
        }

        class AcceptOrder
        {
        }

        class SendOrder
        {
        }

        class OrderSent
        {
        }

        class OrderAccepted
        {
        }
    }
}