using System.Linq;
using NServiceBus;
using NServiceBus.MessageMutator;

#region Mutator
public class MessageEncryptor :
    IMutateTransportMessages
{
    public void MutateIncoming(TransportMessage transportMessage)
    {
        transportMessage.Body = transportMessage.Body.Reverse().ToArray();
    }

    public void MutateOutgoing(object[] messages, TransportMessage transportMessage)
    {
        transportMessage.Body = transportMessage.Body.Reverse().ToArray();
    }
}
#endregion