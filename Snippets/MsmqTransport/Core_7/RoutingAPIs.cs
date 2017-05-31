// ReSharper disable UnusedParameter.Local

namespace Core6.Routing
{
    using NServiceBus;

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

        class AcceptOrder
        {
        }

        class SendOrder
        {
        }
    }
}