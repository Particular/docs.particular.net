using System.Threading.Tasks;
using NServiceBus.MessageMutator;

#region mutate-incoming-messages

public class MutateIncomingMessages :
    IMutateIncomingMessages
{
    public Task MutateIncoming(MutateIncomingMessageContext context)
    {
        var headers = context.Headers;
        headers.Add("MutateIncomingMessages", "ValueMutateIncomingMessages");
        return Task.CompletedTask;
    }
}

#endregion