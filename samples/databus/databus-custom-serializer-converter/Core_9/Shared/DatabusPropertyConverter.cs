using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared
{
#pragma warning disable CS0618 // Type or member is obsolete
    #region DatabusPropertyConverter
    public class DatabusPropertyConverter<T> : JsonConverter<DataBusProperty<T>> where T : class
    {
        public DatabusPropertyConverter(JsonSerializerOptions options)
        {

        }
        public override DataBusProperty<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var sdp = JsonSerializer.Deserialize<SerializableDatabusProperty>(reader.GetString());
            DataBusProperty<T> databusProp = new DataBusProperty<T>();
            databusProp.HasValue = sdp.HasValue;
            databusProp.Key = sdp.Key;
            return databusProp;
        }

        public override void Write(Utf8JsonWriter writer, DataBusProperty<T> value, JsonSerializerOptions options)
        {
            SerializableDatabusProperty sdp = new SerializableDatabusProperty();
            sdp.HasValue = value.HasValue;
            sdp.Key = value.Key;
            string jsonStr = JsonSerializer.Serialize(sdp);
            writer.WriteStringValue(jsonStr);

        }

        private class SerializableDatabusProperty
        {
            public string Key { get; set; }
            public bool HasValue { get; set; }

        }

    }
    #endregion
#pragma warning restore CS0618 // Type or member is obsolete
}
