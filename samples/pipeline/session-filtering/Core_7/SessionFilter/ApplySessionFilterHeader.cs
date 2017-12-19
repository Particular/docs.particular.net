using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region apply-session-header
public class ApplySessionFilterHeader : Behavior<IRoutingContext>
{
    readonly ISessionKeyProvider sessionKeyProvider;

    public ApplySessionFilterHeader(ISessionKeyProvider sessionKeyProvider)
    {
        this.sessionKeyProvider = sessionKeyProvider;
    }

    public override Task Invoke(IRoutingContext context, Func<Task> next)
    {
        context.Message.Headers["NServiceBus.SessionKey"] = sessionKeyProvider.SessionKey;
        return next();
    }
}
#endregion