namespace Snippets5.Azure.Transports.AzureStorageQueues
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region AzureStorageQueueTransportWithAzure 7

            endpointConfiguration.UseTransport<AzureStorageQueueTransport>();

            #endregion
        }

        //TODO: fix when we split azure
        /**
        #region AzureStorageQueueTransportWithAzureHost 7

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
