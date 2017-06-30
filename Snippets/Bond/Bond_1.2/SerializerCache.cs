using System;
using System.Collections.Concurrent;
using Bond;
using Bond.IO.Unsafe;
using Bond.Protocols;


#region SerializerCache
static class SerializerCache
{
    static ConcurrentDictionary<Type, Item> cache = new ConcurrentDictionary<Type, Item>();

    public static Item GetSerializer(Type messageType)
    {
        return cache.GetOrAdd(messageType,
            type => new Item
            (
                new Serializer<CompactBinaryWriter<OutputBuffer>>(type),
                new Deserializer<CompactBinaryReader<InputBuffer>>(type)
            ));
    }

    public class Item
    {
        public readonly Serializer<CompactBinaryWriter<OutputBuffer>> Serializer;
        public readonly Deserializer<CompactBinaryReader<InputBuffer>> Deserializer;

        public Item(
            Serializer<CompactBinaryWriter<OutputBuffer>> serializer,
            Deserializer<CompactBinaryReader<InputBuffer>> deserializer)
        {
            Serializer = serializer;
            Deserializer = deserializer;
        }
    }
}
#endregion