using NServiceBus;
using NServiceBus.MessageMutator;

#region mutate-transport-messages
public class MutateTransportMessages : IMutateTransportMessages
{
    IBus bus;

    public MutateTransportMessages(IBus bus)
    {
        this.bus = bus;
    }

    public void MutateIncoming(TransportMessage transportMessage)
    {
        transportMessage.Headers.Add("MutateTransportMessages_Incoming", "ValueMutateTransportMessages_Incoming");
    }

    public void MutateOutgoing(object[] messages, TransportMessage transportMessage)
    {
        var incomingContext = bus.CurrentMessageContext;
        var incomingMessageId = incomingContext?.Headers["NServiceBus.MessageId"];

        transportMessage.Headers
            .Add("MutateTransportMessages_Outgoing", "ValueMutateTransportMessages_Outgoing");
    }
}
#endregion