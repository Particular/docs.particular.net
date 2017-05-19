using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

class SetPrincipalBehavior : Behavior<IIncomingLogicalMessageContext>
{
    public override async Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        string userName;

        if (context.Headers.TryGetValue("User", out userName))
        {
            var oldPrincipal = Thread.CurrentPrincipal;
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(userName), new string[0]);
            await next().ConfigureAwait(false);
            Thread.CurrentPrincipal = oldPrincipal;
        }
        else
        {
            await next().ConfigureAwait(false);
        }
    }
}