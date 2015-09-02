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

    public void MutateOutgoing(MutateOutgoingMessagesContext context)
    {
        context.SetHeader("MutateOutgoingMessages", "ValueMutateOutgoingMessages");
    }
}
#endregion