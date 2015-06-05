using System.Linq;
using NServiceBus.MessageMutator;
using NServiceBus.Unicast.Transport;

#region Mutator
public class MessageEncryptor : IMutateTransportMessages
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

