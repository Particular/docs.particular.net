using System.Linq;
using System.Threading.Tasks;
using NServiceBus.MessageMutator;

#region Mutator
public class MessageEncryptor :
    IMutateIncomingTransportMessages,
    IMutateOutgoingTransportMessages
{

    public Task MutateIncoming(MutateIncomingTransportMessageContext context)
    {
        context.Body = context.Body.Reverse().ToArray();
        return Task.FromResult(0);
    }

    public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
    {
        context.OutgoingBody = context.OutgoingBody.Reverse().ToArray();
        return Task.FromResult(0);
    }
}
#endregion