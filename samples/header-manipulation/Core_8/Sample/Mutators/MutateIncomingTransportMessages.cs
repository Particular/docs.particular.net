using System.Threading.Tasks;
using NServiceBus.MessageMutator;

#region mutate-incoming-transport-messages
public class MutateIncomingTransportMessages :
    IMutateIncomingTransportMessages
{
    public Task MutateIncoming(MutateIncomingTransportMessageContext context)
    {
        var headers = context.Headers;
        headers.Add("MutateIncomingTransportMessages", "ValueMutateIncomingTransportMessages");
        return Task.CompletedTask;
    }
}
#endregion