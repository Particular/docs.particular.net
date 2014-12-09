using NServiceBus;


public class Transports
{
    public void AllTheTransports()
    {
        #region ConfigureTransports

        var configuration = new BusConfiguration();

        // Configure to use MSMQ 
        configuration.UseTransport<MsmqTransport>();

        // Configure to use AzureStorageQueue
        configuration.UseTransport<AzureStorageQueueTransport>();

        // Configure to use AzureServiceBus
        configuration.UseTransport<AzureServiceBusTransport>();

        // Configure to use SqlServerB
        configuration.UseTransport<SqlServerTransport>();

        // Configure to use Rabbit
        configuration.UseTransport<RabbitMQTransport>();

        #endregion
    }

}