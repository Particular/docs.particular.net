using NServiceBus;
using NServiceBus.MessageMutator;

#region mutate-outgoing-messages
public class MutateOutgoingMessages :
    IMutateOutgoingMessages
{
    IBus bus;

    public MutateOutgoingMessages(IBus bus)
    {
        this.bus = bus;
    }
    public object MutateOutgoing(object message)
    {
        var incomingContext = bus.CurrentMessageContext;
        var incomingMessageId = incomingContext?.Headers["NServiceBus.MessageId"];

        bus.SetMessageHeader(
            msg: message,
            key: "MutateOutgoingMessages",
            value: "ValueMutateOutgoingMessages");
        return message;
    }
}
#endregion