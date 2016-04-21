using System;
using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Converters;
using NServiceBus;
using NServiceBus.Serialization;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region NewtonsoftSerialization

        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

        #endregion
    }

    void CustomSettings(EndpointConfiguration endpointConfiguration)
    {
        #region NewtonsoftCustomSettings

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Converters =
            {
                new IsoDateTimeConverter
                {
                    DateTimeStyles = DateTimeStyles.RoundtripKind
                }
            }
        };
        var serialization = endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        serialization.Settings(settings);

        #endregion
    }

    void CustomReader(EndpointConfiguration endpointConfiguration)
    {
        #region NewtonsoftCustomReader

        var serialization = endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        serialization.ReaderCreator(stream =>
            {
                StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
                return new JsonTextReader(streamReader);
            });

        #endregion
    }

    void CustomWriter(EndpointConfiguration endpointConfiguration)
    {
        #region NewtonsoftCustomWriter

        var serialization = endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        serialization.WriterCreator(stream =>
            {
                StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8);
                return new JsonTextWriter(streamWriter)
                {
                    Formatting = Formatting.None
                };
            });

        #endregion
    }

    void Bson(EndpointConfiguration endpointConfiguration)
    {
        #region NewtonsoftBson

        var serialization = endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        serialization.ReaderCreator(stream => new BsonReader(stream));
        serialization.WriterCreator(stream => new BsonWriter(stream));

        #endregion
    }

    #region NewtonsoftAttributes

    [JsonObject(MemberSerialization.OptIn)]
    public class CreatePersonMessage : IMessage
    {
        // "John Smith"
        [JsonProperty]
        public string Name { get; set; }

        // "2000-12-15T22:11:03"
        [JsonProperty]
        public DateTime BirthDate { get; set; }

        // new Date(976918263055)
        [JsonProperty]
        [JsonConverter(typeof(JavaScriptDateTimeConverter))]
        public DateTime LastModified { get; set; }

        // not serialized because mode is opt-in
        public string Department { get; set; }
    }

    #endregion
}