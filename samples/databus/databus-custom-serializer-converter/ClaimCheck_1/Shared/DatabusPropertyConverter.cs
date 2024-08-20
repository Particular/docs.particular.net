using NServiceBus;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NServiceBus.ClaimCheck;

namespace Shared
{
    #region DatabusPropertyConverter
    public class ClaimCheckPropertyConverter<T> : JsonConverter<ClaimCheckProperty<T>> where T : class
    {
        public ClaimCheckPropertyConverter(JsonSerializerOptions options)
        {

        }
        public override ClaimCheckProperty<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var sdp = JsonSerializer.Deserialize<SerializableDatabusProperty>(reader.GetString());
            ClaimCheckProperty<T> databusProp = new ClaimCheckProperty<T>();
            databusProp.HasValue = sdp.HasValue;
            databusProp.Key = sdp.Key;
            return databusProp;
        }

        public override void Write(Utf8JsonWriter writer, ClaimCheckProperty<T> value, JsonSerializerOptions options)
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
}
