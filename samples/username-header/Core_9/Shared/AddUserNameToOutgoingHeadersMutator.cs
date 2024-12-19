using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;

#region username-header-mutator

public class AddUserNameToOutgoingHeadersMutator(IPrincipalAccessor principalAccessor) :
    IMutateOutgoingTransportMessages
{
    static readonly ILog log = LogManager.GetLogger(nameof(AddUserNameToOutgoingHeadersMutator));

    public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
    {
        if (principalAccessor.CurrentPrincipal?.Identity.Name != null)
        {
            log.Info("Adding CurrentPrincipal user to headers");
            context.OutgoingHeaders[Headers.UserName] = principalAccessor.CurrentPrincipal.Identity.Name;
        }

        return Task.CompletedTask;
    }
}

#endregion