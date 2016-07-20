using NServiceBus;
using NServiceBus.MessageMutator;

#region message-mutator
public class MessageMutator :
    IMessageMutator
{
    IBus bus;

    public MessageMutator(IBus bus)
    {
        this.bus = bus;
    }
    public object MutateOutgoing(object message)
    {
        var incomingContext = bus.CurrentMessageContext;
        var incomingMessageId = incomingContext?.Headers["NServiceBus.MessageId"];

        bus.SetMessageHeader(
            msg: message,
            key: "MessageMutater_Outgoing",
            value: "ValueMessageMutater_Outgoing");
        return message;
    }

    public object MutateIncoming(object message)
    {
        var headers = bus.CurrentMessageContext.Headers;
        headers.Add("MessageMutator_Incoming", "ValueMessageMutator_Incoming");
        return message;
    }
}

#endregion