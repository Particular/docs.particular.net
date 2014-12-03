using System;
using System.Collections.Generic;
using System.IO;
using NServiceBus;
using NServiceBus.Serialization;


#region RegisterCustomSerializer
public class RegisterCustomSerializer : INeedInitialization
{
    public void Customize(BusConfiguration configuration)
    {
        configuration.UseSerialization(typeof(MyCustomSerializer));
    }

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
}
#endregion