using Bond;
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
            sertializationDelegatesBuilder: messageType =>
            {
                return new SerializationDelegates(
                    serialize: (buffer, message) =>
                    {
                        var writer = new CompactBinaryWriter<OutputBuffer>(buffer);
                        var serializer = new Serializer<CompactBinaryWriter<OutputBuffer>>(messageType);
                        serializer.Serialize(message, writer);
                    },
                    deserialize: buffer =>
                    {
                        var reader = new CompactBinaryReader<InputBuffer>(buffer);
                        var deserializer = new Deserializer<CompactBinaryReader<InputBuffer>>(messageType);
                        return deserializer.Deserialize(reader);
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