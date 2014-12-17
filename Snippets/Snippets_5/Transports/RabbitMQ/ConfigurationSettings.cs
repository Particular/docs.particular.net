using global::RabbitMQ.Client;
using NServiceBus;
using NServiceBus.Transports.RabbitMQ;

public class RabbitMQConfigurationSettings
{
    void Basic()
    {
        #region rabbitmq-config-basic 2

        var configuration = new BusConfiguration();
        configuration.UseTransport<RabbitMQTransport>();

        #endregion
    }

    void CustomConnectionString()
    {
        #region rabbitmq-config-connectionstring-in-code 2

        var configuration = new BusConfiguration();
        configuration.UseTransport<RabbitMQTransport>()
            .ConnectionString("My custom connection string");

        #endregion
    }

    void CustomConnectionStringName()
    {
        #region rabbitmq-config-connectionstringname 2

        var configuration = new BusConfiguration();
        configuration.UseTransport<RabbitMQTransport>()
            .ConnectionStringName("MyConnectionStringName");

        #endregion
    }


    void DisableCallbackReceiver()
    {
        #region rabbitmq-config-disablecallbackreceiver 2

        var configuration = new BusConfiguration();
        configuration.UseTransport<RabbitMQTransport>()
            .DisableCallbackReceiver();

        #endregion
    }


    void CallbackReceiverMaxConcurrency()
    {
        #region rabbitmq-config-callbackreceiver-thread-count 2

        var configuration = new BusConfiguration();
        configuration.UseTransport<RabbitMQTransport>()
            .CallbackReceiverMaxConcurrency(10);

        #endregion
    }

    void UseConnectionManager()
    {
        #region rabbitmq-config-useconnectionmanager 2

        var configuration = new BusConfiguration();
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