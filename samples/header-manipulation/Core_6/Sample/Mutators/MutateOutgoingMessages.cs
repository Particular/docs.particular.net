using System.Threading.Tasks;
using NServiceBus.MessageMutator;

#region mutate-outgoing-messages
public class MutateOutgoingMessages :
    IMutateOutgoingMessages
{
    public Task MutateOutgoing(MutateOutgoingMessageContext context)
    {
        var headers = context.OutgoingHeaders;
        headers["MutateOutgoingMessages"] = "ValueMutateOutgoingMessages";
        return Task.CompletedTask;
    }
}
#endregion