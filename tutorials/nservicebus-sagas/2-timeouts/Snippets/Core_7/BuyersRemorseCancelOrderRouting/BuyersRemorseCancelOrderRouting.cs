namespace Core_7.BuyersRemorseCancelOrderRouting
{
    using NServiceBus;

    class EndpointConfig
    {
        public EndpointConfig()
        {
            var endpointConfiguration = new EndpointConfiguration("Fake");
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            var routing = transport.Routing();
            #region BuyersRemorseCancelOrderRouting
            routing.RouteToEndpoint(typeof(CancelOrder), "Sales");
            #endregion
        }
    }

    internal class CancelOrder
    {
    }
}
