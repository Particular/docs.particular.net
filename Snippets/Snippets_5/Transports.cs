using NServiceBus;


public class Transports
{
    public void AllTheTransports()
    {
        #region ConfigureTransports

        BusConfiguration busConfiguration = new BusConfiguration();

        // Configure to use MSMQ 
        busConfiguration.UseTransport<MsmqTransport>();

        // Configure to use AzureStorageQueue
        busConfiguration.UseTransport<AzureStorageQueueTransport>();

        // Configure to use AzureServiceBus
        busConfiguration.UseTransport<AzureServiceBusTransport>();

        // Configure to use SqlServerB
        busConfiguration.UseTransport<SqlServerTransport>();

        // Configure to use Rabbit
        busConfiguration.UseTransport<RabbitMQTransport>();

        #endregion
    }

}