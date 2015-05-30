using NServiceBus;
using NServiceBus.MessageMutator;

#region mutate-transport-messages
public class MutateTransportMessages : IMutateTransportMessages
{
    public void MutateIncoming(TransportMessage transportMessage)
    {
        transportMessage.Headers.Add("KeyFromMutateTransportMessages_Incoming", "ValueFromMutateTransportMessages_Incoming");
    }

    public void MutateOutgoing(object[] messages, TransportMessage transportMessage)
    {
        transportMessage.Headers
            .Add("KeyFromMutateTransportMessages_Outgoing", "ValueFromMutateTransportMessages_Outgoing");
    }
}
#endregion