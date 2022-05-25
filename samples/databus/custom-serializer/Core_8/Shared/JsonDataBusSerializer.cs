using System.Text;

namespace Shared
{
    using Newtonsoft.Json;
    using NServiceBus.DataBus;
    using System;
    using System.IO;

    #region CustomDataBusSerializer

    public class JsonDataBusSerializer :
        IDataBusSerializer
    {
        public void Serialize(object databusProperty, Stream stream)
        {
            using (var writer = CreateNonClosingStreamWriter(stream))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                serializer.Serialize(jsonWriter, databusProperty);
            }
        }

        StreamWriter CreateNonClosingStreamWriter(Stream stream)
            => new StreamWriter(
                stream,
                Encoding.UTF8,
                bufferSize: 1024,
                leaveOpen: true);

        public object Deserialize(Type propertyType, Stream stream)
        {
            using (var reader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(reader))
            {
                return serializer.Deserialize(jsonReader, propertyType);
            }
        }

        JsonSerializer serializer = JsonSerializer.Create(
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            }
        );

        public string ContentType => "application/json";
    }

    #endregion
}