using System;
using RabbitMQ.Client;
using NServiceBus;
using NServiceBus.Transports;
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

    void CustomConnectionStringWithTLS(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-connection-tls

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.ConnectionString(@"host=broker1;UseTls=true;CertPath=C:\CertificatePath\ssl.pfx;CertPassphrase=securePassword");

        #endregion
    }    

    void CallbackReceiverMaxConcurrency(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-callbackreceiver-thread-count

        endpointConfiguration.UseTransport<RabbitMQTransport>();
        endpointConfiguration.LimitMessageProcessingConcurrencyTo(10);

        #endregion
    }

    void CustomIdStrategy(EndpointConfiguration endpointConfiguration)
    {
        #region rabbitmq-config-custom-id-strategy

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.CustomMessageIdStrategy(deliveryArgs =>
            deliveryArgs.BasicProperties.Headers["MyCustomId"].ToString());

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
        transport.UseDirectRoutingTopology(MyRoutingKeyConvention, (address, eventType) => "MyTopic");

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
        #region rabbitmq-custom-breaker-settings-code

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan.FromMinutes(2));

        #endregion
    }

    class MyRoutingTopology : IRoutingTopology
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