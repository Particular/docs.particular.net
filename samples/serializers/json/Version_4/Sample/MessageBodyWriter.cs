using System;
using System.Text;
using NServiceBus;
using NServiceBus.MessageMutator;

#region mutator
public class MessageBodyWriter : 
    IMutateIncomingTransportMessages
{
    public void MutateIncoming(TransportMessage transportMessage)
    {
        string bodyAsString = Encoding.UTF8
            .GetString(transportMessage.Body);
        Console.WriteLine("Serialized Message Body:");
        Console.WriteLine(bodyAsString);
    }
}
#endregion