using NServiceBus;
using NServiceBus.ProtoBuf;
using ProtoBuf.Meta;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region ProtoBufSerialization

        endpointConfiguration.UseSerialization<ProtoBufSerializer>();

        #endregion
    }

    void CustomSettings(EndpointConfiguration endpointConfiguration)
    {
        #region ProtoBufCustomSettings

        var runtimeTypeModel = TypeModel.Create();
        runtimeTypeModel.IncludeDateTimeKind = true;
        var serialization = endpointConfiguration.UseSerialization<ProtoBufSerializer>();
        serialization.RuntimeTypeModel(runtimeTypeModel);

        #endregion
    }

    void ContentTypeKey(EndpointConfiguration endpointConfiguration)
    {
        #region ProtoBufContentTypeKey

        var serialization = endpointConfiguration.UseSerialization<ProtoBufSerializer>();
        serialization.ContentTypeKey("custom-key");

        #endregion
    }

}