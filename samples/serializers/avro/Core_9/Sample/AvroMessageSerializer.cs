using System;
using System.Collections.Generic;
using System.IO;
using Avro.IO;
using Avro.Reflect;
using Avro.Specific;
using NServiceBus.Serialization;

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
            // Avro is not able to infer message types from the body content
            return [];
        }

        var messages = new List<object>();
        foreach (var messageType in messageTypes)
        {
            var schema = schemaRegistry.GetSchema(messageType);
            var reader = new ReflectDefaultReader(messageType, schema, schema, classCache);
            using var stream = new ReadOnlyStream(body);
            var message = reader.Read(null, schema, schema, new JsonDecoder(schema, stream));
            messages.Add(message);
        }

        return messages.ToArray();
    }
}