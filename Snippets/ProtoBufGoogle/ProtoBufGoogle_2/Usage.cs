using NServiceBus;
using NServiceBus.ProtoBufGoogle;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region ProtoBufSerialization

        endpointConfiguration.UseSerialization<ProtoBufGoogleSerializer>();

        #endregion
    }

    void ContentTypeKey(EndpointConfiguration endpointConfiguration)
    {
        #region ProtoBufContentTypeKey

        var serialization = endpointConfiguration.UseSerialization<ProtoBufGoogleSerializer>();
        serialization.ContentTypeKey("custom-key");

        #endregion
    }

}