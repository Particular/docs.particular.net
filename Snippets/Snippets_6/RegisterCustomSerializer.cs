namespace Snippets6
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using NServiceBus;
    using NServiceBus.MessageInterfaces;
    using NServiceBus.Serialization;
    using NServiceBus.Settings;

    public class RegisterCustomSerializer
    {
        #region RegisterCustomSerializer

        public void ConfigureBus()
        {
            BusConfiguration busConfiguration = null;
            busConfiguration.UseSerialization<MyCustomSerialization>();
        }

        class MyCustomSerialization : SerializationDefinition
        {
            protected override Func<IMessageMapper, IMessageSerializer> Configure(ReadOnlySettings settings)
            {
                return mapper => new MyCustomSerializer();
            }
        }
        #endregion

        #region CustomSerializer

        class MyCustomSerializer : IMessageSerializer
        {
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