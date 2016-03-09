namespace Snippets5.Transports.RabbitMQ
{
    using System;
    using global::RabbitMQ.Client;
    using NServiceBus;
    using NServiceBus.Transports.RabbitMQ;
    using NServiceBus.Transports.RabbitMQ.Routing;

    public class Usage
    {
        void Basic()
        {
            #region rabbitmq-config-basic

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<RabbitMQTransport>();

            #endregion
        }

        void CustomConnectionString()
        {
            #region rabbitmq-config-connectionstring-in-code

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<RabbitMQTransport>()
                .ConnectionString("My custom connection string");

            #endregion
        }

        void CustomConnectionStringName()
        {
            #region rabbitmq-config-connectionstringname

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<RabbitMQTransport>()
                .ConnectionStringName("MyConnectionStringName");

            #endregion
        }


        void DisableCallbackReceiver()
        {
            #region rabbitmq-config-disablecallbackreceiver

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<RabbitMQTransport>()
                .DisableCallbackReceiver();

            #endregion
        }


        void CallbackReceiverMaxConcurrency()
        {
            #region rabbitmq-config-callbackreceiver-thread-count

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<RabbitMQTransport>()
                .CallbackReceiverMaxConcurrency(10);

            #endregion
        }
        void CustomIdStrategy()
        {
            #region rabbitmq-config-custom-id-strategy

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<RabbitMQTransport>()
                .CustomMessageIdStrategy(deliveryArgs => 
                    deliveryArgs.BasicProperties.Headers["MyCustomId"].ToString());

            #endregion
        }
        void UseConnectionManager()
        {
            #region rabbitmq-config-useconnectionmanager

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<RabbitMQTransport>()
                .UseConnectionManager<MyConnectionManager>();

            #endregion
        }

        void UseDirectRoutingTopology()
        {
            #region rabbitmq-config-usedirectroutingtopology

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<RabbitMQTransport>()
                .UseDirectRoutingTopology();

            #endregion
        }

        void UseDirectRoutingTopologyWithCustomConventions()
        {
            #region rabbitmq-config-usedirectroutingtopologywithcustomconventions

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<RabbitMQTransport>()
                .UseDirectRoutingTopology(MyRoutingKeyConvention,(address,eventType) => "MyTopic");

            #endregion
        }

        string MyRoutingKeyConvention(Type type)
        {
            throw new NotImplementedException();
        }

        void UseRoutingTopology()
        {
            #region rabbitmq-config-useroutingtopology

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<RabbitMQTransport>()
                .UseRoutingTopology<MyRoutingTopology>();

            #endregion
        }
        class MyRoutingTopology : IRoutingTopology
        {
            public void SetupSubscription(IModel channel, Type type, string subscriberName)
            {
                throw new NotImplementedException();
            }

            public void TeardownSubscription(IModel channel, Type type, string subscriberName)
            {
                throw new NotImplementedException();
            }

            public void Publish(IModel channel, Type type, TransportMessage message, IBasicProperties properties)
            {
                throw new NotImplementedException();
            }

            public void Send(IModel channel, Address address, TransportMessage message, IBasicProperties properties)
            {
                throw new NotImplementedException();
            }

            public void Initialize(IModel channel, string main)
            {
                throw new NotImplementedException();
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

