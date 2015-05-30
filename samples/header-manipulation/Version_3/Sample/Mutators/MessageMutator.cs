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
        bus.OutgoingHeaders
            .Add("KeyFromMutateOutgoing_Outgoing", "ValueFromMutateOutgoing_Outgoing");
        return message;
    }

    public object MutateIncoming(object message)
    {
        bus.CurrentMessageContext
            .Headers
            .Add("KeyFromMessageMutator_Incoming", "ValueFromMessageMutator_Incoming");
        return message;
    }
}
#endregion