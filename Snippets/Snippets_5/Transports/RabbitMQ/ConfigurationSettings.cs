namespace MyServer.Transports.RabbitMQ
{
    using global::RabbitMQ.Client;
    using NServiceBus;
    using NServiceBus.Transports.RabbitMQ;

    public class ConfigurationSettings
    {
        void Basic()
        {
            var configuration = new BusConfiguration();


            #region rabbitmq-config-basic

            configuration.UseTransport<RabbitMQTransport>();

            #endregion

        }

        void CustomConnectionString()
        {
            var configuration = new BusConfiguration();


            #region rabbitmq-config-connectionstring-in-code

            configuration.UseTransport<RabbitMQTransport>()
                .ConnectionString("My custom connection string");

            #endregion

        }

        void CustomConnectionStringName()
        {
            var configuration = new BusConfiguration();


            #region rabbitmq-config-connectionstringname

            configuration.UseTransport<RabbitMQTransport>()
                .ConnectionStringName("MyOwnName");

            #endregion

        }


        void DisableCallbackReceiver()
        {
            var configuration = new BusConfiguration();


            #region rabbitmq-config-disablecallbackreceiver

            configuration.UseTransport<RabbitMQTransport>()
                .DisableCallbackReceiver();

            #endregion

        }


        void CallbackReceiverMaxConcurrency()
        {
            var configuration = new BusConfiguration();


            #region rabbitmq-config-callbackreceiver-thread-count
            
            configuration.UseTransport<RabbitMQTransport>()
                .CallbackReceiverMaxConcurrency(10);

            #endregion

        }

        void UseConnectionManager()
        {
            var configuration = new BusConfiguration();

            var connectionManager = new MyConnectionManager();

            #region rabbitmq-config-useconnectionmanager
           
            configuration.UseTransport<RabbitMQTransport>()
                .UseConnectionManager<MyConnectionManager>();

            #endregion

        }
        class MyConnectionManager : IManageRabbitMqConnections
        {
            public IConnection GetPublishConnection()
            {
                throw new System.NotImplementedException();
            }

            public IConnection GetConsumeConnection()
            {
                throw new System.NotImplementedException();
            }

            public IConnection GetAdministrationConnection()
            {
                throw new System.NotImplementedException();
            }
        }
    }

 
}