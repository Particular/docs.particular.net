using NServiceBus;

public class RabbitMQConfigurationSettings
{
    void Basic()
    {
        #region rabbitmq-config-basic 1

        Configure.With()
            .UseTransport<NServiceBus.RabbitMQ>();

        #endregion

    }

    void CustomConnectionString()
    {


        #region rabbitmq-config-connectionstring-in-code 1

        Configure.With()
            .UseTransport<NServiceBus.RabbitMQ>(() => "My custom connection string");
        #endregion

    }

    void CustomConnectionStringName()
    {
        #region rabbitmq-config-connectionstringname 1

        Configure.With()
            .UseTransport<NServiceBus.RabbitMQ>("MyConnectionStringName");

        #endregion

    }

}