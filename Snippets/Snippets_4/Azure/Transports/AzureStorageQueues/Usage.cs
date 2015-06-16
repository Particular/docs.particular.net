namespace Snippets4.Azure.Transports.AzureStorageQueues
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
            #region AzureStorageQueueTransportWithAzure 5

            Configure.With()
                .UseTransport<AzureStorageQueue>();

            #endregion
        }

        #region AzureStorageQueueTransportWithAzureHost 5

        public class EndpointConfig : IConfigureThisEndpoint, UsingTransport<AzureStorageQueue> { }

        #endregion
    }
}