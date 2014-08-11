using NServiceBus;


public class Transports
{
    public void AllTheTransports()
    {
        #region ConfigureTransportsV5

        // Configure to use MSMQ 
        Configure.With(b => b.UseTransport<Msmq>());

        // Configure to use AzureStorageQueue
        Configure.With(b => b.UseTransport<AzureStorageQueue>());

        // Configure to use AzureServiceBus
        Configure.With(b => b.UseTransport<AzureServiceBus>());

        // Configure to use SqlServer
        Configure.With(b => b.UseTransport<SqlServer>());

        #endregion
    }

}