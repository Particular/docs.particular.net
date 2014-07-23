using NServiceBus;


public class Transports
{
    public void AllTheTransports()
    {
        // start code ConfigureTransportsV4

        // Configure to use MSMQ 
        Configure.With().UseTransport<Msmq>();

        // Configure to use AzureStorageQueue
        Configure.With().UseTransport<AzureStorageQueue>();

        // Configure to use AzureServiceBus
        Configure.With().UseTransport<AzureServiceBus>();

        // Configure to use SqlServer
        Configure.With().UseTransport<SqlServer>();

        // end code ConfigureTransportsV4
    }

}