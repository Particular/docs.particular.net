namespace Snippets5
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using NServiceBus;
    using NServiceBus.Serialization;

    public class RegisterCustomSerializer
    {
        RegisterCustomSerializer(BusConfiguration busConfiguration)
        {
            #region RegisterCustomSerializer

            busConfiguration.UseSerialization(typeof(MyCustomSerializer));

            #endregion
        }

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