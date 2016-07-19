using System.Threading.Tasks;
using NServiceBus.MessageMutator;

#region mutate-outgoing-messages
public class MutateOutgoingMessages :
    IMutateOutgoingMessages
{
    public Task MutateOutgoing(MutateOutgoingMessageContext context)
    {
        context.OutgoingHeaders["MutateOutgoingMessages"] = "ValueMutateOutgoingMessages";
        return Task.FromResult(0);
    }
}
#endregion