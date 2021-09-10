using System.Threading.Tasks;
using NServiceBus.MessageMutator;

#region mutate-outgoing-transport-messages

public class MutateOutgoingTransportMessages :
    IMutateOutgoingTransportMessages
{
    public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
    {
        var headers = context.OutgoingHeaders;
        headers["MutateOutgoingTransportMessages"] = "ValueMutateOutgoingTransportMessages";
        return Task.CompletedTask;
    }
}

#endregion