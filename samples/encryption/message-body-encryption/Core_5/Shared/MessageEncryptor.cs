using System.Linq;
using NServiceBus;
using NServiceBus.MessageMutator;
using NServiceBus.Unicast.Messages;

#region Mutator
public class MessageEncryptor :
    IMutateTransportMessages
{
    public void MutateIncoming(TransportMessage transportMessage)
    {
        transportMessage.Body = transportMessage.Body.Reverse().ToArray();
    }

    public void MutateOutgoing(LogicalMessage logicalMessage, TransportMessage transportMessage)
    {
        transportMessage.Body = transportMessage.Body.Reverse().ToArray();
    }
}
#endregion