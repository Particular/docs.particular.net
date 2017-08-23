using NServiceBus;

partial class Upgrade
{
#pragma warning disable CS0618
    void UseRoutingTopology4_0(EndpointConfiguration endpointConfiguration)
    {
        #region 40to41rabbitmq-useroutingtopology 4.0

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseRoutingTopology<MyRoutingTopology>();

        #endregion
    }
#pragma warning restore CS0618

    void UseRoutingTopology4_1(EndpointConfiguration endpointConfiguration)
    {
        #region 40to41rabbitmq-useroutingtopology 4.1

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseRoutingTopology(
            topologyFactory: createDurableExchangesAndQueues =>
            {
                return new MyRoutingTopology();
            });

        #endregion
    }

}