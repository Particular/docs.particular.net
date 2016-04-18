using NServiceBus;
using NServiceBus.MessageMutator;

#region message-mutator
public class MessageMutator : IMessageMutator
{
    IBus bus;

    public MessageMutator(IBus bus)
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

        bus.SetMessageHeader(message, "MessageMutater_Outgoing", "ValueMessageMutater_Outgoing");
        return message;
    }

    public object MutateIncoming(object message)
    {
        bus.CurrentMessageContext
            .Headers
            .Add("MessageMutator_Incoming", "ValueMessageMutator_Incoming");
        return message;
    }
}

#endregion