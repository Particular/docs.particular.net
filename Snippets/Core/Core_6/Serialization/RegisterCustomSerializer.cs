namespace Core6.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using NServiceBus;
    using NServiceBus.MessageInterfaces;
    using NServiceBus.Serialization;
    using NServiceBus.Settings;

    class RegisterCustomSerializer
    {
        void Customize(EndpointConfiguration endpointConfiguration)
        {
            #region RegisterCustomSerializer

            // register serializer additionally configuring settings
            endpointConfiguration.UseSerialization<MyCustomSerializerDefinition>().Settings("settingsValue");

            #endregion
        }

        #region CustomSerializer

        class MyCustomSerializerDefinition :
            SerializationDefinition
        {
            public override Func<IMessageMapper, IMessageSerializer> Configure(ReadOnlySettings settings)
            {
                return mapper => new MyCustomSerializer(settings.GetOrDefault<string>("MyCustomSerializer.Settings"));
            }
        }

        class MyCustomSerializer :
            IMessageSerializer
        {
            public MyCustomSerializer(string settingsValue)
            {
                // Add code initializing serializer on the basis of settingsValue
                throw new NotImplementedException();
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

        #endregion
    }
}