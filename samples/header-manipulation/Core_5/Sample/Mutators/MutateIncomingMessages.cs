using NServiceBus;
using NServiceBus.MessageMutator;

#region mutate-incoming-messages
public class MutateIncomingMessages :
    IMutateIncomingMessages
{
    IBus bus;

    public MutateIncomingMessages(IBus bus)
    {
        this.bus = bus;
    }

    public object MutateIncoming(object message)
    {
        bus.CurrentMessageContext
            .Headers
            .Add("MutateIncomingMessages", "ValueMutateIncomingMessages");
        return message;
    }
}
#endregion