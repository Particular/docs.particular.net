using Avro.IO;
using Avro.Reflect;
using NServiceBus.Serialization;

#region serializer-implementation
public class AvroMessageSerializer(SchemaRegistry schemaRegistry, ClassCache classCache) : IMessageSerializer
{
    public string ContentType => "avro/json";

    public void Serialize(object message, Stream stream)
    {
        var messageType = message.GetType();
        var schema = schemaRegistry.GetSchema(messageType);
        var writer = new ReflectDefaultWriter(messageType, schema, classCache);

        var encoder = new JsonEncoder(schema, stream);

        writer.Write(message, encoder);

        encoder.Flush();
    }

    public object[] Deserialize(ReadOnlyMemory<byte> body, IList<Type> messageTypes = null)
    {
        if (messageTypes == null)
        {
            throw new MessageDeserializationException(
                "Avro is not able to infer message types from the body content only," +
                "the NServiceBus.EnclosedMessageTypes header must be present");
        }

        var messages = new List<object>();
        foreach (var messageType in messageTypes)
        {
            try
            {
                var schema = schemaRegistry.GetSchema(messageType);
                var reader = new ReflectDefaultReader(messageType, schema, schema, classCache);
                using var stream = new ReadOnlyStream(body);
                var message = reader.Read(null, schema, schema, new JsonDecoder(schema, stream));
                messages.Add(message);
            }
            catch (KeyNotFoundException)
            {
                throw new MessageDeserializationException(
                    $"No schema found for message type {messageType.FullName}");
            }
        }

        return messages.ToArray();
    }
}
#endregion