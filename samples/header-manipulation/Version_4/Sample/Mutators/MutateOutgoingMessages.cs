using NServiceBus;
using NServiceBus.MessageMutator;

#region mutate-outgoing-messages
public class MutateOutgoingMessages : IMutateOutgoingMessages
{
    IBus bus;

    public MutateOutgoingMessages(IBus  bus)
    {
        this.bus = bus;
    }
    public object MutateOutgoing(object message)
    {
        bus.OutgoingHeaders
            .Add("KeyFromMutateOutgoingMessages", "ValueFromMutateOutgoingMessages");
        return message;
    }
}
#endregion