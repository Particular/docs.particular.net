using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;

#region Mutator

public class UsernameMutator :
    IMutateOutgoingTransportMessages,
    IMutateIncomingTransportMessages
{
    static ILog log = LogManager.GetLogger("Handler");

    public Task MutateIncoming(MutateIncomingTransportMessageContext context)
    {
        log.Info("Reading headers to set Thread.CurrentPrincipal");
        var identityName = context.Headers["UserName"];
        Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(identityName), new string[] { });

        return Task.CompletedTask;
    }

    public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
    {
        log.Info("Adding Thread.CurrentPrincipal user to headers");
        context.OutgoingHeaders["UserName"] = Thread.CurrentPrincipal.Identity.Name;
        return Task.CompletedTask;
    }
}

#endregion