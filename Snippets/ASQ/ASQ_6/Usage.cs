using NServiceBus;

class Usage
{
    Usage(BusConfiguration busConfiguration)
    {
        #region AzureStorageQueueTransportWithAzure

        var transport = busConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString("DefaultEndpointsProtocol=https;AccountName={youraccount};AccountKey={yourkey};");

        #endregion
    }

    #region AzureStorageQueueTransportWithAzureHost

    public class EndpointConfig :
        IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration busConfiguration)
        {
            var transport = busConfiguration.UseTransport<AzureStorageQueueTransport>();
            transport.ConnectionString("DefaultEndpointsProtocol=https;AccountName={youraccount};AccountKey={yourkey};");
        }
    }

    #endregion
}