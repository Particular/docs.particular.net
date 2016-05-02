using NServiceBus;

class Usage
{
    Usage(BusConfiguration busConfiguration)
    {
        #region AzureStorageQueueTransportWithAzure

        busConfiguration.UseTransport<AzureStorageQueueTransport>();

        #endregion
    }

    #region AzureStorageQueueTransportWithAzureHost

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration busConfiguration)
        {
            busConfiguration.UseTransport<AzureStorageQueueTransport>();
        }
    }

    #endregion
}