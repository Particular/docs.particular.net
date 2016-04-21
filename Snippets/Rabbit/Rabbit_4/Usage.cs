namespace Rabbit_4
{
    using System;
    using RabbitMQ.Client;
    using NServiceBus;
    using NServiceBus.Transports;
    using NServiceBus.Transports.RabbitMQ.Routing;

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

            endpointConfiguration.UseTransport<RabbitMQTransport>()
                .ConnectionString("My custom connection string");

            #endregion
        }

        void CustomConnectionStringName(EndpointConfiguration endpointConfiguration)
        {
            #region rabbitmq-config-connectionstringname

            endpointConfiguration.UseTransport<RabbitMQTransport>()
                .ConnectionStringName("MyConnectionStringName");

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

            endpointConfiguration.UseTransport<RabbitMQTransport>()
                .CustomMessageIdStrategy(deliveryArgs =>
                    deliveryArgs.BasicProperties.Headers["MyCustomId"].ToString());

            #endregion
        }

        void UseDirectRoutingTopology(EndpointConfiguration endpointConfiguration)
        {
            #region rabbitmq-config-usedirectroutingtopology

            endpointConfiguration.UseTransport<RabbitMQTransport>()
                .UseDirectRoutingTopology();

            #endregion
        }

        void UseDirectRoutingTopologyWithCustomConventions(EndpointConfiguration endpointConfiguration)
        {
            #region rabbitmq-config-usedirectroutingtopologywithcustomconventions

            endpointConfiguration.UseTransport<RabbitMQTransport>()
                .UseDirectRoutingTopology(MyRoutingKeyConvention, (address, eventType) => "MyTopic");

            #endregion
        }

        string MyRoutingKeyConvention(Type type)
        {
            throw new NotImplementedException();
        }

        void UseRoutingTopology(EndpointConfiguration endpointConfiguration)
        {
            #region rabbitmq-config-useroutingtopology

            endpointConfiguration.UseTransport<RabbitMQTransport>()
                .UseRoutingTopology<MyRoutingTopology>();

            #endregion
        }

        void UseCustomCircuitBreakerSettings(EndpointConfiguration endpointConfiguration)
        {
            #region rabbitmq-custom-breaker-settings-code

            endpointConfiguration.UseTransport<RabbitMQTransport>()
                .TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan.FromMinutes(2));

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
}