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

            #region AzureStorageQueueConfigCodeOnly

            endpointConfiguration.UseTransport<AzureStorageQueueTransport>()
                .ConnectionString("azure-storage-connection-string")
                .BatchSize(20)
                .MaximumWaitTimeWhenIdle(1000)
                .PeekInterval(100);

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