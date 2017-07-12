using MsgPack.Serialization;
using NServiceBus;
using MessagePackSerializer = NServiceBus.MessagePack.MessagePackSerializer;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region MessagePackSerialization

        endpointConfiguration.UseSerialization<MessagePackSerializer>();

        #endregion
    }

    void CustomSettings(EndpointConfiguration endpointConfiguration)
    {
        #region MessagePackCustomSettings

        var context = new SerializationContext
        {
            DefaultDateTimeConversionMethod = DateTimeConversionMethod.UnixEpoc
        };
        var serialization = endpointConfiguration.UseSerialization<MessagePackSerializer>();
        serialization.Context(context);

        #endregion
    }

    void ContentTypeKey(EndpointConfiguration endpointConfiguration)
    {
        #region MessagePackContentTypeKey

        var serialization = endpointConfiguration.UseSerialization<MessagePackSerializer>();
        serialization.ContentTypeKey("custom-key");

        #endregion
    }

}
