using NServiceBus;

partial class Upgrade
{
    void CustomConnectionStringName(EndpointConfiguration endpointConfiguration)
    {
        #region 4to5CustomConnectionStringName

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.ConnectionStringName("MyConnectionStringName");

        #endregion
    }

    void UseDirectRoutingTopologyWithCustomConventions(EndpointConfiguration endpointConfiguration)
    {
        #region 4to5usedirectroutingtopology

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseDirectRoutingTopology(
            routingKeyConvention: type => "myroutingkey",
            exchangeNameConvention: (address, eventType) => "MyTopic");

        #endregion
    }
}