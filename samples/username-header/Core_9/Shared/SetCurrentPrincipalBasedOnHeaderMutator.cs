using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus.MessageMutator;

#region set-principal-from-header-mutator
public class SetCurrentPrincipalBasedOnHeaderMutator :
    IMutateIncomingTransportMessages
{
  
    readonly IPrincipalAccessor principalAccessor;
    private readonly ILogger<SetCurrentPrincipalBasedOnHeaderMutator> logger;

    public SetCurrentPrincipalBasedOnHeaderMutator(IPrincipalAccessor principalAccessor, ILogger<SetCurrentPrincipalBasedOnHeaderMutator> logger)
    {
        this.principalAccessor = principalAccessor;
        this.logger = logger;
    }

    public Task MutateIncoming(MutateIncomingTransportMessageContext context)
    {
        if (context.Headers.TryGetValue("UserName", out var userNameHeader))
        {
            logger.LogInformation("Adding CurrentPrincipal user from headers");
            var identity = new GenericIdentity(userNameHeader);
            principalAccessor.CurrentPrincipal = new GenericPrincipal(identity, new string[0]);
        }

        return Task.CompletedTask;
    }
}
#endregion