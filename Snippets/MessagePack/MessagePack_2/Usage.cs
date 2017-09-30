using MessagePack.Resolvers;
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
        #region MessagePackResolver

        var serialization = endpointConfiguration.UseSerialization<MessagePackSerializer>();
        serialization.Resolver(ContractlessStandardResolver.Instance);

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
