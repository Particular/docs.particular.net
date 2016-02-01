namespace Snippets4.Transports.RabbitMQ
{
    using NServiceBus;

    public class Usage
    {
        void Basic()
        {
            #region rabbitmq-config-basic

            Configure configure = Configure.With();
            configure.UseTransport<RabbitMQ>();

            #endregion
        }

        void CustomConnectionString()
        {
            #region rabbitmq-config-connectionstring-in-code

            Configure configure = Configure.With();
            configure.UseTransport<RabbitMQ>(() => "My custom connection string");
            #endregion
        }

        void CustomConnectionStringName()
        {
            #region rabbitmq-config-connectionstringname

            Configure configure = Configure.With();
            configure.UseTransport<RabbitMQ>("MyConnectionStringName");

            #endregion
        }
    }
}