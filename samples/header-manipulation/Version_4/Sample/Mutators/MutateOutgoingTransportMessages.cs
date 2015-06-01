using NServiceBus;
using NServiceBus.MessageMutator;

#region mutate-outgoing-transport-messages
public class MutateOutgoingTransportMessages : IMutateOutgoingTransportMessages
{
    IBus bus;

    public MutateOutgoingTransportMessages(IBus bus)
    {
        this.bus = bus;
    }

    public void MutateOutgoing(object[] messages, TransportMessage transportMessage)
    {
        IMessageContext incomingContext = bus.CurrentMessageContext;
        if (incomingContext != null)
        {
            string incomingMessageId = incomingContext.Headers["NServiceBus.MessageId"];
        }

        transportMessage.Headers
            .Add("MutateOutgoingTransportMessages", "ValueMutateOutgoingTransportMessages");
    }
}
#endregion