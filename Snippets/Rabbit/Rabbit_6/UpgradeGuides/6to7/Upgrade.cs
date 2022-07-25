using NServiceBus;

partial class Upgrade
{
    void UseConventionalRoutingTopology(EndpointConfiguration endpointConfiguration)
    {
        #region 6to7conventional

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseConventionalRoutingTopology();

        #endregion
    }

    void UseDirectRoutingTopology(EndpointConfiguration endpointConfiguration)
    {
        #region 6to7direct

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseDirectRoutingTopology();

        #endregion
    }
}