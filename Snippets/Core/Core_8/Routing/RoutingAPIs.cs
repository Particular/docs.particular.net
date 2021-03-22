// ReSharper disable UnusedParameter.Local

namespace Core8.Routing
{
    using NServiceBus;

    class RoutingAPIs
    {
        void LogicalRouting(EndpointConfiguration endpointConfiguration)
        {
            #region Routing-Logical

            var routing = endpointConfiguration.UseTransport(new TransportDefinition());

            routing.RouteToEndpoint(
                assembly: typeof(AcceptOrder).Assembly,
                destination: "Sales");

            routing.RouteToEndpoint(
                assembly: typeof(AcceptOrder).Assembly,
                @namespace: "PriorityMessages",
                destination: "Preferred");

            routing.RouteToEndpoint(
                messageType: typeof(SendOrder),
                destination: "Sending");

            #endregion
        }

        void SubscribeRouting(EndpointConfiguration endpointConfiguration)
        {
            #region Routing-RegisterPublisher

            var routing = endpointConfiguration.UseTransport(new TransportDefinition());

            routing.RegisterPublisher(
                assembly: typeof(OrderAccepted).Assembly,
                publisherEndpoint: "Sales");

            routing.RegisterPublisher(
                assembly: typeof(OrderAccepted).Assembly,
                @namespace: "PriorityMessages",
                publisherEndpoint: "Preferred");

            routing.RegisterPublisher(
                eventType: typeof(OrderSent),
                publisherEndpoint: "Sending");
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