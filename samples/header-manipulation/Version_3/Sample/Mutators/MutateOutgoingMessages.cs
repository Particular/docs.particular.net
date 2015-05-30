using NServiceBus;
using NServiceBus.MessageMutator;

#region mutate-outgoing-messages
public class MutateOutgoingMessages : IMutateOutgoingMessages
{
    IBus bus;

    public MutateOutgoingMessages(IBus bus)
    {
        this.bus = bus;
    }

    public object MutateOutgoing(object message)
    {
        IMessageContext incomingContext = bus.CurrentMessageContext;
        if (incomingContext != null)
        {
            string incomingMessageId = incomingContext.Headers["NServiceBus.MessageId"];
        }

        message.SetHeader("MutateOutgoingMessages", "ValueMutateOutgoingMessages");
        return message;
    }
}
#endregion