using System;
using System.Collections.Generic;
using System.IO;
using Avro.IO;
using Avro.Reflect;
using NServiceBus.Serialization;

namespace Sample;

public class AvroMessageSerializer(SchemaCache schemaCache, ClassCache classCache) : IMessageSerializer
{
    public string ContentType => "avro/binary";

    public void Serialize(object message, Stream stream)
    {
        var schema = schemaCache.GetSchema(message.GetType());
        var writer = new ReflectDefaultWriter(message.GetType(), schema, classCache);

        writer.Write(message, new BinaryEncoder(stream));
    }

    public object[] Deserialize(ReadOnlyMemory<byte> body, IList<Type> messageTypes = null)
    {
        var messages = new List<object>();
        foreach (var messageType in messageTypes)
        {
            var schema = schemaCache.GetSchema(messageType);
            var reader = new ReflectDefaultReader(messageType, schema, schema, classCache);
            using var stream = new ReadOnlyStream(body);
            var message = reader.Read(null, schema, schema, new BinaryDecoder(stream));
            messages.Add(message);
        }

        return messages.ToArray();
    }
}