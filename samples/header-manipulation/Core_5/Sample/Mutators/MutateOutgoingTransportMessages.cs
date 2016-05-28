using NServiceBus;
using NServiceBus.MessageMutator;
using NServiceBus.Unicast.Messages;

#region mutate-outgoing-transport-messages
public class MutateOutgoingTransportMessages : IMutateOutgoingTransportMessages
{
    IBus bus;

    public MutateOutgoingTransportMessages(IBus bus)
    {
        this.bus = bus;
    }

    public void MutateOutgoing(LogicalMessage logicalMessage, TransportMessage transportMessage)
    {
        var incomingContext = bus.CurrentMessageContext;
        var incomingMessageId = incomingContext?.Headers["NServiceBus.MessageId"];

        transportMessage.Headers
            .Add("MutateOutgoingTransportMessages", "ValueMutateOutgoingTransportMessages");
    }
}
#endregion