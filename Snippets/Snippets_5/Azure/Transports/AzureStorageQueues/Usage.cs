namespace Snippets5.Azure.Transports.AzureStorageQueues
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
            #region AzureStorageQueueTransportWithAzure 6

            BusConfiguration busConfiguration = new BusConfiguration();
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