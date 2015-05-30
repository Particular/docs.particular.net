using NServiceBus;
using NServiceBus.MessageMutator;

#region mutate-outgoing-transport-messages
public class MutateOutgoingTransportMessages : IMutateOutgoingTransportMessages
{
    public void MutateOutgoing(object[] messages, TransportMessage transportMessage)
    {
        transportMessage.Headers
            .Add("KeyFromMutateOutgoingTransportMessages", "ValueFromMutateOutgoingTransportMessages");
    }
}
#endregion