namespace Snippets5.Azure.Transports.AzureStorageQueues
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region AzureStorageQueueTransportWithAzure

            endpointConfiguration.UseTransport<AzureStorageQueueTransport>();

            #endregion
        }

        //TODO: fix when we split azure
        /**
        #region AzureStorageQueueTransportWithAzureHost

        public class EndpointConfig : IConfigureThisEndpoint
        {
            public void Customize(EndpointConfiguration endpointConfiguration)
            {
                endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
            }
        }

        #endregion
    **/
    }
}
