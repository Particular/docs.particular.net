using NServiceBus;
using NServiceBus.MessageMutator;
using NServiceBus.Unicast.Messages;

#region mutate-transport-messages

public class MutateTransportMessages : IMutateTransportMessages
{
    public void MutateIncoming(TransportMessage transportMessage)
    {
        transportMessage.Headers
            .Add("MutateTransportMessages_Incoming", "ValueMutateTransportMessages_Incoming");
    }

    public void MutateOutgoing(LogicalMessage logicalMessage, TransportMessage transportMessage)
    {
        transportMessage.Headers
            .Add("MutateTransportMessages_Outgoing", "ValueMutateTransportMessages_Outgoing");
    }
}

#endregion