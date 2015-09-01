namespace Snippets4.Azure.Transports.AzureStorageQueues
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
            #region AzureStorageQueueTransportWithAzure 5

            Configure configure = Configure.With();
            configure.UseTransport<AzureStorageQueue>();

            #endregion
        }

        #region AzureStorageQueueTransportWithAzureHost 5

        public class EndpointConfig : IConfigureThisEndpoint, UsingTransport<AzureStorageQueue> { }

        #endregion
    }
}