using NServiceBus;

public class Usage
{
    void Basic(Configure configure)
    {
        #region rabbitmq-config-basic
        configure.UseTransport<NServiceBus.RabbitMQ>();

        #endregion
    }

    void CustomConnectionString(Configure configure)
    {
        #region rabbitmq-config-connectionstring-in-code

        configure.UseTransport<NServiceBus.RabbitMQ>(() => "My custom connection string");
        #endregion
    }

    void CustomConnectionStringName(Configure configure)
    {
        #region rabbitmq-config-connectionstringname

        configure.UseTransport<NServiceBus.RabbitMQ>("MyConnectionStringName");

        #endregion
    }
}
