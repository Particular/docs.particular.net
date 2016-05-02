using System;
using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Converters;
using NServiceBus;
using NServiceBus.Newtonsoft.Json;

class Usage
{
    Usage(BusConfiguration busConfiguration)
    {
        #region NewtonsoftSerialization

        busConfiguration.UseSerialization<NewtonsoftSerializer>();

        #endregion
    }

    void CustomSettings(BusConfiguration busConfiguration)
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
        var serialization = busConfiguration.UseSerialization<NewtonsoftSerializer>();
        serialization.Settings(settings);

        #endregion
    }

    void CustomReader(BusConfiguration busConfiguration)
    {
        #region NewtonsoftCustomReader

        var serialization = busConfiguration.UseSerialization<NewtonsoftSerializer>();
        serialization.ReaderCreator(stream =>
        {
            StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
            return new JsonTextReader(streamReader);
        });

        #endregion
    }

    void CustomWriter(BusConfiguration busConfiguration)
    {
        #region NewtonsoftCustomWriter

        var serialization = busConfiguration.UseSerialization<NewtonsoftSerializer>();
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

    void Bson(BusConfiguration busConfiguration)
    {
        #region NewtonsoftBson

        var serialization = busConfiguration.UseSerialization<NewtonsoftSerializer>();
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