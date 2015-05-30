using NServiceBus;
using NServiceBus.MessageMutator;
using NServiceBus.Unicast.Messages;

#region mutate-outgoing-transport-messages
public class MutateOutgoingTransportMessages : IMutateOutgoingTransportMessages
{
    public void MutateOutgoing(LogicalMessage logicalMessage, TransportMessage transportMessage)
    {
        transportMessage.Headers
            .Add("KeyFromMutateOutgoingTransportMessages", "ValueFromMutateOutgoingTransportMessages");
    }
}
#endregion