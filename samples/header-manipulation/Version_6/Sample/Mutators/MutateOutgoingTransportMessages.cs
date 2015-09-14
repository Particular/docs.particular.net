using System.Threading.Tasks;
using NServiceBus.MessageMutator;

#region mutate-outgoing-transport-messages
public class MutateOutgoingTransportMessages : IMutateOutgoingTransportMessages
{
    public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
    {
        context.OutgoingHeaders["MutateOutgoingTransportMessages"]= "ValueMutateOutgoingTransportMessages";
        return Task.FromResult(0);
    }
}
#endregion