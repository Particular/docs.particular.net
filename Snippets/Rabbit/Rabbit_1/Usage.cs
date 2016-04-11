namespace Rabbit_1
{
    using NServiceBus;

    public class Usage
    {
        void Basic(Configure configure)
        {
            #region rabbitmq-config-basic

            configure.UseTransport<RabbitMQ>();

            #endregion
        }

        void CustomConnectionString(Configure configure)
        {
            #region rabbitmq-config-connectionstring-in-code

            configure.UseTransport<RabbitMQ>(() => "My custom connection string");
            #endregion
        }

        void CustomConnectionStringName(Configure configure)
        {
            #region rabbitmq-config-connectionstringname

            configure.UseTransport<RabbitMQ>("MyConnectionStringName");

            #endregion
        }
    }
}