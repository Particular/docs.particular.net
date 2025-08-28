using System.Text;

namespace Shared
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Bson;
    using NServiceBus.ClaimCheck;
    using System;
    using System.IO;

    #region CustomDataBusSerializer

    public class BsonClaimCheckSerializer :
        IClaimCheckSerializer
    {
        public void Serialize(object databusProperty, Stream stream)
        {
            using (var writer = CreateNonClosingBinaryWriter(stream))
            using (var bsonWriter = new BsonDataWriter(writer))
            {
                serializer.Serialize(bsonWriter, databusProperty);
            }
        }

        public object Deserialize(Type propertyType, Stream stream)
        {
            using (var jsonReader = new BsonDataReader(stream))
            {
                return serializer.Deserialize(jsonReader, propertyType);
            }
        }

        static BinaryWriter CreateNonClosingBinaryWriter(Stream stream)
        {
            return new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        }

        static JsonSerializer serializer = new JsonSerializer
        {
            TypeNameHandling = TypeNameHandling.All
        };

        public string ContentType => "application/bson";
    }

    #endregion
}