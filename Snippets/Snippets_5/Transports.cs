using NServiceBus;


public class Transports
{
    public void AllTheTransports()
    {
        #region ConfigureTransportsV5

        var configuration = new BusConfiguration();

        // Configure to use MSMQ 
        configuration.UseTransport<MsmqTransport>();

        // Configure to use AzureStorageQueue
        configuration.UseTransport<AzureStorageQueue>();

        // Configure to use AzureServiceBus
        configuration.UseTransport<AzureServiceBus>();

        // Configure to use SqlServer
        configuration.UseTransport<SqlServer>();

        #endregion
    }

}