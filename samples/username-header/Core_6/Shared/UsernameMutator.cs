using System.Threading;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;

#region Mutator
public class UsernameMutator :
    IMutateOutgoingTransportMessages
{
    static ILog logger = LogManager.GetLogger("Handler");

    public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
    {
        logger.Info("Adding Thread.CurrentPrincipal user to headers");
        context.OutgoingHeaders["UserName"] = Thread.CurrentPrincipal.Identity.Name;
        return Task.FromResult(0);
    }
}
#endregion