using System;
using NServiceBus;
using NServiceBus.Transport;
using NServiceBus.Transport.RabbitMQ;
using RabbitMQ.Client;

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
        transport.UseRoutingTopology(createDurableExchangesAndQueues => new MyRoutingTopology());

        #endregion
    }

    class MyRoutingTopology :
       IRoutingTopology
    {
        public void SetupSubscription(IModel channel, Type type, string subscriberName)
        {
        }

        public void TeardownSubscription(IModel channel, Type type, string subscriberName)
        {
        }

        public void Publish(IModel channel, Type type, OutgoingMessage message, IBasicProperties properties)
        {
        }

        public void Send(IModel channel, string address, OutgoingMessage message, IBasicProperties properties)
        {
        }

        public void RawSendInCaseOfFailure(IModel channel, string address, byte[] body, IBasicProperties properties)
        {
        }

        public void Initialize(IModel channel, string main)
        {
        }
    }
}

