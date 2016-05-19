using NServiceBus;

class Usage
{
    Usage(BusConfiguration busConfiguration)
    {
        #region AzureStorageQueueTransportWithAzure

        busConfiguration.UseTransport<AzureStorageQueueTransport>()
                        .ConnectionString("DefaultEndpointsProtocol=https;AccountName={youraccount};AccountKey={yourkey};");

        #endregion
    }

    #region AzureStorageQueueTransportWithAzureHost

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration busConfiguration)
        {
            busConfiguration.UseTransport<AzureStorageQueueTransport>()
                            .ConnectionString("DefaultEndpointsProtocol=https;AccountName={youraccount};AccountKey={yourkey};");
        }
    }

    #endregion
}