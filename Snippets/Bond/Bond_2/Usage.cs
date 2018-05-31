using Bond.IO.Unsafe;
using Bond.Protocols;
using NServiceBus;
using NServiceBus.Bond;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region BondSerialization

        endpointConfiguration.UseSerialization<BondSerializer>();

        #endregion
    }

    void SerializationDelegates(EndpointConfiguration endpointConfiguration)
    {
        #region BondSerializationDelegates

        var serialization = endpointConfiguration.UseSerialization<BondSerializer>();
        serialization.SerializationDelegates(
            serializationDelegatesBuilder: messageType =>
            {
                var item = SerializerCache.GetSerializer(messageType);
                return new SerializationDelegates(
                    serialize: (buffer, message) =>
                    {
                        var writer = new CompactBinaryWriter<OutputBuffer>(buffer);
                        item.Serializer.Serialize(message, writer);
                    },
                    deserialize: buffer =>
                    {
                        var reader = new CompactBinaryReader<InputBuffer>(buffer);
                        return item.Deserializer.Deserialize(reader);
                    });
            });

        #endregion
    }

    void ContentTypeKey(EndpointConfiguration endpointConfiguration)
    {
        #region BondContentTypeKey

        var serialization = endpointConfiguration.UseSerialization<BondSerializer>();
        serialization.ContentTypeKey("custom-key");

        #endregion
    }

}