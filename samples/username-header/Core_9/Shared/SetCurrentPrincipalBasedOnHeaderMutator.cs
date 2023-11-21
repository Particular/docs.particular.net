using System.Security.Principal;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;

#region set-principal-from-header-mutator
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
        if (context.Headers.TryGetValue("UserName", out var userNameHeader))
        {
            log.Info("Adding CurrentPrincipal user from headers");
            var identity = new GenericIdentity(userNameHeader);
            principalAccessor.CurrentPrincipal = new GenericPrincipal(identity, new string[0]);
        }

        return Task.CompletedTask;
    }
}
#endregion