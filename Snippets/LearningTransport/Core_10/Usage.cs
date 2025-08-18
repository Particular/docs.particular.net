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
        #region StorageDirectory

        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        transport.StorageDirectory("PathToStoreTransportFiles");

        #endregion
    }

    void NoPayloadSizeRestriction(EndpointConfiguration endpointConfiguration)
    {
        #region NoPayloadSizeRestriction

        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        transport.NoPayloadSizeRestriction();

        #endregion
    }
}
