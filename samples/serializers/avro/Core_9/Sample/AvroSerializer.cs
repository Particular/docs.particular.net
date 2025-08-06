using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Avro;
using Avro.Reflect;
using NServiceBus.MessageInterfaces;
using NServiceBus.Serialization;
using NServiceBus.Settings;
using NServiceBus.Unicast.Messages;

namespace Sample;

public class SchemaCache
{
    public Schema GetSchema(Type getType) => schemaCache[getType];

    readonly IDictionary<Type, Schema> schemaCache = new Dictionary<Type, Schema>();

    public void Add(Type messageType, Schema schema)
    {
        schemaCache[messageType] = schema;
    }
}

public class AvroSerializer : SerializationDefinition
{
    public override Func<IMessageMapper, IMessageSerializer> Configure(IReadOnlySettings settings)
    {
        //for now use reflection to generate the schemas
        var registry = settings.Get<MessageMetadataRegistry>();
        var messageTypes = registry.GetAllMessages().Select(m => m.MessageType);
        var schemaCache = new SchemaCache();
        var assembly = Assembly.GetExecutingAssembly();

        foreach (var messageType in messageTypes)
        {
            string manifestNamespace = "Sample.";
            var schemaResourceName = manifestNamespace + messageType.Name + ".avsc";
            using (var stream = assembly.GetManifestResourceStream(schemaResourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException($"Resource '{schemaResourceName}' not found in assembly '{assembly.FullName}'.");
                }

                // Load the schema from the embedded resource
                using var reader = new StreamReader(stream);
                var schemaJson = reader.ReadToEnd();
                // Parse and cache the schema
                schemaCache.Add(messageType, Schema.Parse(schemaJson));
            }

        }

        return _ => new AvroMessageSerializer(schemaCache, new ClassCache());
    }
}