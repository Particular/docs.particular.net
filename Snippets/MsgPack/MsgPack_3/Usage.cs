using MsgPack.Serialization;
using NServiceBus;
using MsgPackSerializer = NServiceBus.MsgPack.MsgPackSerializer;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region MsgPackSerialization

        endpointConfiguration.UseSerialization<MsgPackSerializer>();

        #endregion
    }

    void CustomSettings(EndpointConfiguration endpointConfiguration)
    {
        #region MsgPackCustomSettings

        var context = new SerializationContext
        {
            DefaultDateTimeConversionMethod = DateTimeConversionMethod.UnixEpoc
        };
        var serialization = endpointConfiguration.UseSerialization<MsgPackSerializer>();
        serialization.Context(context);

        #endregion
    }

    void ContentTypeKey(EndpointConfiguration endpointConfiguration)
    {
        #region MsgPackContentTypeKey

        var serialization = endpointConfiguration.UseSerialization<MsgPackSerializer>();
        serialization.ContentTypeKey("custom-key");

        #endregion
    }
}