using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

class PropagateOutgoingTenantIdBehavior :
    Behavior<IOutgoingLogicalMessageContext>
{
    public override Task Invoke(IOutgoingLogicalMessageContext context, Func<Task> next)
    {
        #region PropagateTenantIdOutgoing

        if (context.Extensions.TryGet("TenantId", out string tenant))
        {
            context.Headers["TenantId"] = tenant;
        }
        return next();

        #endregion
    }
}