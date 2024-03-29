﻿using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region NewtonsoftSerialization

        endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();

        #endregion
    }

    void CustomSettings(EndpointConfiguration endpointConfiguration)
    {
        #region NewtonsoftCustomSettings

        var settings = new JsonSerializerSettings
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
        var serialization = endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
        serialization.Settings(settings);

        #endregion
    }

    void CustomReader(EndpointConfiguration endpointConfiguration)
    {
        #region NewtonsoftCustomReader

        var serialization = endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
        serialization.ReaderCreator(stream =>
        {
            var streamReader = new StreamReader(stream, Encoding.UTF8);
            return new JsonTextReader(streamReader);
        });

        #endregion
    }

    void CustomWriter(EndpointConfiguration endpointConfiguration)
    {
        #region NewtonsoftCustomWriter

        var noBomEncoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: false);

        var serialization = endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
        serialization.WriterCreator(stream =>
        {
            var streamWriter = new StreamWriter(stream, noBomEncoding);
            return new JsonTextWriter(streamWriter)
            {
                Formatting = Formatting.None
            };
        });

        #endregion
    }

    void ContentTypeKey(EndpointConfiguration endpointConfiguration)
    {
        #region NewtonsoftContentTypeKey

        var serialization = endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
        serialization.ContentTypeKey("custom-key");

        #endregion
    }

    void Bson(EndpointConfiguration endpointConfiguration)
    {
        #region NewtonsoftBson

        var serialization = endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
        serialization.ReaderCreator(stream => new BsonDataReader(stream));
        serialization.WriterCreator(stream => new BsonDataWriter(stream));

        #endregion
    }

    #region NewtonsoftAttributes

    [JsonObject(MemberSerialization.OptIn)]
    public class CreatePersonMessage :
        IMessage
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

    void KnownTypesBinderConfig(EndpointConfiguration endpointConfiguration)
    {
        #region KnownTypesBinderConfig
        endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>()
            .Settings(new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                SerializationBinder = new SkipAssemblyNameForMessageTypesBinder(new[]
                {
                    typeof(MyNativeIntegrationMessage)
                })
            });

        #endregion
    }

    #region KnownTypesBinder
    class SkipAssemblyNameForMessageTypesBinder : ISerializationBinder
    {
        Type[] messageTypes;

        public SkipAssemblyNameForMessageTypesBinder(Type[] messageTypes)
        {
            this.messageTypes = messageTypes;
        }

        public Type BindToType(string assemblyName, string typeName)
        {
            return messageTypes.FirstOrDefault(messageType => messageType.FullName == typeName);
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = serializedType.Assembly.FullName;
            typeName = serializedType.FullName;
        }
    }
    #endregion

    class MyNativeIntegrationMessage
    {

    }
}
