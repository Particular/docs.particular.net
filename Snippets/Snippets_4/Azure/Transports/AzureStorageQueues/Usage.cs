namespace Snippets4.Azure.Transports.AzureStorageQueues
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configure)
        {
            #region AzureStorageQueueTransportWithAzure 5

            configure.UseTransport<AzureStorageQueue>();

            #endregion
        }

        #region AzureStorageQueueTransportWithAzureHost 5

        public class EndpointConfig : IConfigureThisEndpoint, UsingTransport<AzureStorageQueue> { }

        #endregion
    }
}