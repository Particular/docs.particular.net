using NServiceBus;

namespace Core_9.BuyersRemorseCancelOrderRouting;

class EndpointConfig
{
    public EndpointConfig()
    {
        var endpointConfiguration = new EndpointConfiguration("Fake");
        var routing = endpointConfiguration.UseTransport(new LearningTransport());
        #region BuyersRemorseCancelOrderRouting
        routing.RouteToEndpoint(typeof(CancelOrder), "Sales");
        #endregion
    }
}

internal class CancelOrder
{
}