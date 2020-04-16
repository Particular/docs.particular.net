using NServiceBus;

partial class Upgrade
{
    void UseDirectRoutingTopologyWithCustomConventions(EndpointConfiguration endpointConfiguration)
    {
        #region 4to5usedirectroutingtopology

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseDirectRoutingTopology(
            routingKeyConvention: type => "myroutingkey",
            exchangeNameConvention: () => "MyTopic");

        #endregion
    }
}