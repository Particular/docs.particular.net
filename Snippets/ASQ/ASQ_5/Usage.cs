using NServiceBus;

class Usage
{
    Usage(Configure configure)
    {
        #region AzureStorageQueueTransportWithAzure

        configure.UseTransport<AzureStorageQueue>();

        #endregion
    }

    #region AzureStorageQueueTransportWithAzureHost

    public class EndpointConfig :
        IConfigureThisEndpoint,
        UsingTransport<AzureStorageQueue>
    {
    }

    #endregion
}