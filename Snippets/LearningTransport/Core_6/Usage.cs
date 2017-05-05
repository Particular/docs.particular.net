namespace LearningTransport
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region LearningTransport

            endpointConfiguration.UseTransport<LearningTransport>();

            #endregion
        }

        void StorageDirectory(EndpointConfiguration endpointConfiguration)
        {
            #region LearningTransport-StorageDirectory

            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            transport.StorageDirectory("PathToStoreTransportFiles");

            #endregion
        }
    }
}