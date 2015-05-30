using NServiceBus.MessageMutator;
using NServiceBus.Unicast.Transport;

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