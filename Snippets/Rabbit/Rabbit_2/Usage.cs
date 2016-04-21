namespace Rabbit_2
{
    using System;
    using global::RabbitMQ.Client;
    using NServiceBus;
    using NServiceBus.Transports.RabbitMQ;
    using NServiceBus.Transports.RabbitMQ.Routing;

    class Usage
    {
        void Basic(BusConfiguration busConfiguration)
        {
            #region rabbitmq-config-basic

            busConfiguration.UseTransport<RabbitMQTransport>();

            #endregion
        }

        void CustomConnectionString(BusConfiguration busConfiguration)
        {
            #region rabbitmq-config-connectionstring-in-code

            busConfiguration.UseTransport<RabbitMQTransport>()
                .ConnectionString("My custom connection string");

            #endregion
        }

        void CustomConnectionStringName(BusConfiguration busConfiguration)
        {
            #region rabbitmq-config-connectionstringname

            busConfiguration.UseTransport<RabbitMQTransport>()
                .ConnectionStringName("MyConnectionStringName");

            #endregion
        }


        void DisableCallbackReceiver(BusConfiguration busConfiguration)
        {
            #region rabbitmq-config-disable-callback-receiver

            busConfiguration.UseTransport<RabbitMQTransport>()
                .DisableCallbackReceiver();

            #endregion
        }


        void CallbackReceiverMaxConcurrency(BusConfiguration busConfiguration)
        {
            #region rabbitmq-config-callbackreceiver-thread-count

            busConfiguration.UseTransport<RabbitMQTransport>()
                .CallbackReceiverMaxConcurrency(10);

            #endregion
        }
        void CustomIdStrategy(BusConfiguration busConfiguration)
        {
            #region rabbitmq-config-custom-id-strategy

            busConfiguration.UseTransport<RabbitMQTransport>()
                .CustomMessageIdStrategy(deliveryArgs =>
                    deliveryArgs.BasicProperties.Headers["MyCustomId"].ToString());

            #endregion
        }
        void UseConnectionManager(BusConfiguration busConfiguration)
        {
            #region rabbitmq-config-useconnectionmanager

            busConfiguration.UseTransport<RabbitMQTransport>()
                .UseConnectionManager<MyConnectionManager>();

            #endregion
        }

        void UseDirectRoutingTopology(BusConfiguration busConfiguration)
        {
            #region rabbitmq-config-usedirectroutingtopology

            busConfiguration.UseTransport<RabbitMQTransport>()
                .UseDirectRoutingTopology();

            #endregion
        }

        void UseDirectRoutingTopologyWithCustomConventions(BusConfiguration busConfiguration)
        {
            #region rabbitmq-config-usedirectroutingtopologywithcustomconventions

            busConfiguration.UseTransport<RabbitMQTransport>()
                .UseDirectRoutingTopology(MyRoutingKeyConvention,(address,eventType) => "MyTopic");

            #endregion
        }

        string MyRoutingKeyConvention(Type type)
        {
            throw new NotImplementedException();
        }

        void UseRoutingTopology(BusConfiguration busConfiguration)
        {
            #region rabbitmq-config-useroutingtopology

            busConfiguration.UseTransport<RabbitMQTransport>()
                .UseRoutingTopology<MyRoutingTopology>();

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

            public void Publish(IModel channel, Type type, TransportMessage message, IBasicProperties properties)
            {
            }

            public void Send(IModel channel, Address address, TransportMessage message, IBasicProperties properties)
            {
            }

            public void Initialize(IModel channel, string main)
            {
            }
        }
        class MyConnectionManager : IManageRabbitMqConnections
        {
            public IConnection GetPublishConnection()
            {
                throw new NotImplementedException();
            }

            public IConnection GetConsumeConnection()
            {
                throw new NotImplementedException();
            }

            public IConnection GetAdministrationConnection()
            {
                throw new NotImplementedException();
            }
        }
    }
}

