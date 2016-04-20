namespace ASQ_5
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configure)
        {
            #region AzureStorageQueueTransportWithAzure

            configure.UseTransport<AzureStorageQueue>();

            #endregion
        }

        //TODO: fix when azure host is split
        /**
        #region AzureStorageQueueTransportWithAzureHost

        public class EndpointConfig : IConfigureThisEndpoint, UsingTransport<AzureStorageQueue> { }

        #endregion
    **/
    }
}
