using Avro;

public class SchemaRegistry
{
    public Schema GetSchema(Type messageType)
    {
        if (!schemaCache.TryGetValue(messageType, out var schema))
        {
            throw new KeyNotFoundException($"No schema found for {messageType.Name}");
        }

        return schema;
    }

    public void Add(Type messageType, Schema schema)
    {
        schemaCache[messageType] = schema;
    }

    readonly IDictionary<Type, Schema> schemaCache = new Dictionary<Type, Schema>();
}