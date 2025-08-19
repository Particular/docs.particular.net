using System.Text;
using NServiceBus.ClaimCheck;

namespace Shared
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Bson;
    using NServiceBus.DataBus;
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

        BinaryWriter CreateNonClosingBinaryWriter(Stream stream) =>
            new BinaryWriter(
                stream,
                Encoding.UTF8,
                leaveOpen: true);


        JsonSerializer serializer = JsonSerializer.Create(
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            }
        );

        public string ContentType => "application/bson";
    }

    #endregion
}