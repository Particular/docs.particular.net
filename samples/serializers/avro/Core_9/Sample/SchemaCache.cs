using System;
using System.Collections.Generic;
using Avro;

public class SchemaCache
{
    public Schema GetSchema(Type getType) => schemaCache[getType];

    public void Add(Type messageType, Schema schema)
    {
        schemaCache[messageType] = schema;
    }

    readonly IDictionary<Type, Schema> schemaCache = new Dictionary<Type, Schema>();
}