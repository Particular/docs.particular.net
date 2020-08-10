namespace Core8.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using NServiceBus;
    using NServiceBus.Configuration.AdvancedExtensibility;
    using NServiceBus.MessageInterfaces;
    using NServiceBus.Serialization;
    using NServiceBus.Settings;

    class RegisterCustomSerializer
    {
        void Customize(EndpointConfiguration endpointConfiguration)
        {
            #region RegisterCustomSerializer

            // register serializer additionally configuring settings
            var serialization = endpointConfiguration.UseSerialization<MyCustomSerializerDefinition>();
            serialization.Settings("settingsValue");

            #endregion
        }
    }

    #region CustomSerializer

    class MyCustomSerializerDefinition :
        SerializationDefinition
    {
        public const string Key = "MyCustomSerializer.Settings";

        public override Func<IMessageMapper, IMessageSerializer> Configure(ReadOnlySettings settings)
        {
            return mapper =>
            {
                var value = settings.GetOrDefault<string>(Key);
                return new MyCustomSerializer(value);
            };
        }
    }

    class MyCustomSerializer :
        IMessageSerializer
    {
        public MyCustomSerializer(string settingsValue)
        {
            // Add code initializing serializer on the basis of settingsValue
        }

        public void Serialize(object message, Stream stream)
        {
            // Add code to serialize message
            throw new NotImplementedException();
        }

        public object[] Deserialize(Stream stream, IList<Type> messageTypes = null)
        {
            // Add code to deserialize message
            throw new NotImplementedException();
        }

        public string ContentType
        {
            get { throw new NotImplementedException(); }
        }
    }

    static class MyCustomSerializerConfigurationExtensions
    {
        public static void Settings(this SerializationExtensions<MyCustomSerializerDefinition> config, string value)
        {
            var settingsHolder = config.GetSettings();
            settingsHolder.Set(MyCustomSerializerDefinition.Key, value);
        }
    }

    #endregion
}