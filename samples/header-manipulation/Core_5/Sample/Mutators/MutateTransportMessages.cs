using NServiceBus;
using NServiceBus.MessageMutator;
using NServiceBus.Unicast.Messages;

#region mutate-transport-messages

public class MutateTransportMessages :
    IMutateTransportMessages
{
    public void MutateIncoming(TransportMessage transportMessage)
    {
        var headers = transportMessage.Headers;
        headers.Add("MutateTransportMessages_Incoming", "ValueMutateTransportMessages_Incoming");
    }

    public void MutateOutgoing(LogicalMessage logicalMessage, TransportMessage transportMessage)
    {
        var headers = transportMessage.Headers;
        headers.Add("MutateTransportMessages_Outgoing", "ValueMutateTransportMessages_Outgoing");
    }
}

#endregion