namespace Snippets6.Serialization
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Bson;
    using Newtonsoft.Json.Converters;
    using NServiceBus;
    using NServiceBus.Serialization;

    public class NewtonsoftSerializerUsage
    {
        public void Simple()
        {
            #region NewtonsoftSerialization 0.3-pre

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

            #endregion
        }

        public void CustomSettings()
        {
            #region NewtonsoftCustomSettings 0.3-pre

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
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>()
                .Settings(settings);

            #endregion
        }

        public void CustomReader()
        {
            #region NewtonsoftCustomReader 0.3-pre

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>()
                .ReaderCreator(stream =>
                {
                    StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
                    return new JsonTextReader(streamReader);
                });

            #endregion
        }

        public void CustomWriter()
        {
            #region NewtonsoftCustomWriter 0.3-pre

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>()
                .WriterCreator(stream =>
                {
                    StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8);
                    return new JsonTextWriter(streamWriter)
                    {
                        Formatting = Formatting.None
                    };
                });

            #endregion
        }
        public void Bson()
        {
            #region NewtonsoftBson 0.3-pre

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            SerializationExtentions<NewtonsoftSerializer> serialization =
                endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            serialization.ReaderCreator(stream => new BsonReader(stream));
            serialization.WriterCreator(stream => new BsonWriter(stream));

            #endregion
        }

        #region NewtonsoftAttributes 0.3-pre

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
}