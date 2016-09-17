using System;
using RabbitMQ.Client;
using NServiceBus;
using NServiceBus.Transport;
using NServiceBus.Transport.RabbitMQ;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-basic

        endpointConfiguration.UseTransport<RabbitMQTransport>();

        #endregion
    }

    void CustomConnectionString(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-connectionstring-in-code

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.ConnectionString("My custom connection string");

        #endregion
    }

    void CustomConnectionStringName(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-connectionstringname

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.ConnectionStringName("MyConnectionStringName");

        #endregion
    }

    void CustomIdStrategy(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-custom-id-strategy

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.CustomMessageIdStrategy(
            customIdStrategy: deliveryArgs =>
            {
                var headers = deliveryArgs.BasicProperties.Headers;
                return headers["MyCustomId"].ToString();
            });

        #endregion
    }

    void UseDirectRoutingTopology(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-usedirectroutingtopology

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseDirectRoutingTopology();

        #endregion
    }

    void UseDirectRoutingTopologyWithCustomConventions(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-usedirectroutingtopologywithcustomconventions

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseDirectRoutingTopology(
            routingKeyConvention: MyRoutingKeyConvention,
            exchangeNameConvention: (address, eventType) => "MyTopic");

        #endregion
    }

    void DisablePublisherConfirms(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-disablepublisherconfirms

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UsePublisherConfirms(false);

        #endregion
    }

    string MyRoutingKeyConvention(Type type)
    {
        throw new NotImplementedException();
    }

    void UseRoutingTopology(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-useroutingtopology

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseRoutingTopology<MyRoutingTopology>();

        #endregion
    }

    void UseCustomCircuitBreakerSettings(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-custom-breaker-settings-time-to-wait-before-triggering-code

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan.FromMinutes(2));

        #endregion
    }

    void PrefetchMultiplier(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-prefetch-multiplier

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.PrefetchMultiplier(4);

        #endregion
    }

    void PrefetchCount(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-prefetch-count

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.PrefetchCount(100);

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