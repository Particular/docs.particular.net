using System.Text;

namespace Shared
{
    using System.IO;
    using Newtonsoft.Json;
    using NServiceBus.DataBus;

    #region CustomDataBusSerializer

    public class JsonDataBusSerializer : IDataBusSerializer
    {
        public void Serialize(object databusProperty, Stream stream)
        {
            using (var writer = new StreamWriter(stream, Encoding.UTF8, bufferSize: 1024, leaveOpen: true))
            using (var jsonWriter = new JsonTextWriter(writer))
                serializer.Serialize(jsonWriter, databusProperty);
        }

        public object Deserialize(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(reader))
                return serializer.Deserialize(jsonReader);
        }

        private JsonSerializer serializer = JsonSerializer.Create(
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            }
        );
    }

    #endregion
}
