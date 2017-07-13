using System;
using System.IO;
using System.Xml.Linq;
using global::Newtonsoft.Json;
#region XContainerJsonConverter
using NewtonsoftJsonSerializer = global::Newtonsoft.Json.JsonSerializer;

class XContainerJsonConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, NewtonsoftJsonSerializer serializer)
    {
        var container = (XContainer) value;
        writer.WriteValue(container.ToString(SaveOptions.DisableFormatting));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, NewtonsoftJsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        if (reader.TokenType != JsonToken.String)
        {
            throw new Exception($"Unexpected token or value when parsing XContainer. Token: {reader.TokenType}, Value: {reader.Value}");
        }

        var value = (string) reader.Value;
        if (objectType == typeof(XDocument))
        {
            try
            {
                return XDocument.Load(new StringReader(value));
            }
            catch (Exception exception)
            {
                throw new Exception($"Error parsing XContainer string: {reader.Value}", exception);
            }
        }

        return XElement.Load(new StringReader(value));
    }


    public override bool CanConvert(Type objectType)
    {
        return typeof(XContainer).IsAssignableFrom(objectType);
    }
}
#endregion