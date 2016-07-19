using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

internal class PropagateOutgoingTenantIdBehavior :
    Behavior<IOutgoingLogicalMessageContext>
{
    public override Task Invoke(IOutgoingLogicalMessageContext context, Func<Task> next)
    {
        #region PropagateTenantIdOutgoing

        string tenant;
        if (context.Extensions.TryGet("TenantId", out tenant))
        {
            context.Headers["TenantId"] = tenant;
        }
        return next();

        #endregion
    }

    public class Registration :
        RegisterStep
    {
        public Registration()
            : base("PropagateOutgoingTenantId", typeof(PropagateOutgoingTenantIdBehavior), "Sets the tenant header from the context bag into the messages header.")
        {
        }
    }
}