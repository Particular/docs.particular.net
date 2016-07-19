using System.Threading.Tasks;
using NServiceBus.MessageMutator;

#region mutate-incoming-messages
public class MutateIncomingMessages :
    IMutateIncomingMessages
{
    public Task MutateIncoming(MutateIncomingMessageContext context)
    {
        context
            .Headers
            .Add("MutateIncomingMessages", "ValueMutateIncomingMessages");
        return Task.FromResult(0);
    }
}
#endregion