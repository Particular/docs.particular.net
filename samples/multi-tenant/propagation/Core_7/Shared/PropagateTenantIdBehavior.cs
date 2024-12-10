using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region PropagateTenantId

public class PropagateTenantIdBehavior :
    Behavior<IOutgoingLogicalMessageContext>
{
    public override Task Invoke(IOutgoingLogicalMessageContext context, Func<Task> next)
    {
        if (context.Extensions.TryGet("TenantId", out string tenant))
        {
            context.Headers["tenant_id"] = tenant;
        }
        return next();

    }
}

#endregion
