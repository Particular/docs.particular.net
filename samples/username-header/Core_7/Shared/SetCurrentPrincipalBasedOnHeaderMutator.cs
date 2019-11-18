using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;

public class SetCurrentPrincipalBasedOnHeaderMutator :
    IMutateIncomingTransportMessages
{
    static ILog log = LogManager.GetLogger("Handler");
    readonly IPrincipalAccessor principalAccessor;

    public SetCurrentPrincipalBasedOnHeaderMutator(IPrincipalAccessor principalAccessor)
    {
        this.principalAccessor = principalAccessor;
    }

    public Task MutateIncoming(MutateIncomingTransportMessageContext context)
    {
        log.Info("Adding CurrentPrincipal user from headers");
        var identity = new GenericIdentity(context.Headers["UserName"]);
        principalAccessor.CurrentPrincipal = new GenericPrincipal(identity, new string[0]);
        return Task.CompletedTask;
    }
}
