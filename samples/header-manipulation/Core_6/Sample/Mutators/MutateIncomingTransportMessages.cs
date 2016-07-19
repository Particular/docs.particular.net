using System.Threading.Tasks;
using NServiceBus.MessageMutator;

#region mutate-incoming-transport-messages
public class MutateIncomingTransportMessages :
    IMutateIncomingTransportMessages
{
    public Task MutateIncoming(MutateIncomingTransportMessageContext context)
    {
        context.Headers
            .Add("MutateIncomingTransportMessages", "ValueMutateIncomingTransportMessages");
        return Task.FromResult(0);
    }
}
#endregion