using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus.MessageMutator;

#region username-header-mutator

public class AddUserNameToOutgoingHeadersMutator:
    IMutateOutgoingTransportMessages
{
    readonly IPrincipalAccessor principalAccessor;
    private readonly ILogger<AddUserNameToOutgoingHeadersMutator> logger;

    public AddUserNameToOutgoingHeadersMutator(IPrincipalAccessor principalAccessor, ILogger<AddUserNameToOutgoingHeadersMutator> logger)
    {
        this.principalAccessor = principalAccessor;
        this.logger = logger;
    }

    public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
    {
        if (principalAccessor.CurrentPrincipal?.Identity.Name != null)
        {

            logger.LogInformation("Adding CurrentPrincipal user to headers");
            context.OutgoingHeaders["UserName"] = principalAccessor.CurrentPrincipal.Identity.Name;
        }

        return Task.CompletedTask;
    }
}

#endregion