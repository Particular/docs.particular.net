using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;

#region username-header-mutator

public class AddUserNameToOutgoingHeadersMutator :
    IMutateOutgoingTransportMessages
{
    static ILog log = LogManager.GetLogger("Handler");
    readonly IPrincipalAccessor principalAccessor;

    public AddUserNameToOutgoingHeadersMutator(IPrincipalAccessor principalAccessor)
    {
        this.principalAccessor = principalAccessor;
    }

    public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
    {
        if (principalAccessor.CurrentPrincipal?.Identity.Name != null)
        {
            log.Info("Adding CurrentPrincipal user to headers");
            context.OutgoingHeaders["UserName"] = principalAccessor.CurrentPrincipal.Identity.Name;
        }

        return Task.CompletedTask;
    }
}

#endregion