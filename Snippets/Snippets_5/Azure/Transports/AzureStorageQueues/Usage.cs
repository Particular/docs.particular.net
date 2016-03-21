namespace Snippets5.Azure.Transports.AzureStorageQueues
{
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region AzureStorageQueueTransportWithAzure 6

            busConfiguration.UseTransport<AzureStorageQueueTransport>();

            #endregion
        }

        #region AzureStorageQueueTransportWithAzureHost 6

        public class EndpointConfig : IConfigureThisEndpoint
        {
            public void Customize(BusConfiguration busConfiguration)
            {
                busConfiguration.UseTransport<AzureStorageQueueTransport>();
            }
        }

        #endregion
    }
}