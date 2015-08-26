using System.Linq;
using NServiceBus;
using NServiceBus.MessageMutator;

#region Mutator
public class MessageEncryptor : IMutateIncomingTransportMessages, IMutateOutgoingTransportMessages
{
    public void MutateIncoming(TransportMessage transportMessage)
    {
        transportMessage.Body = transportMessage.Body.Reverse().ToArray();
    }

  
    public void MutateOutgoing(MutateOutgoingTransportMessagesContext context)
    {
        context.Body = context.Body.Reverse().ToArray();
    }
}
#endregion