using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

class PropagateOutgoingTenantIdBehavior :
    Behavior<IOutgoingLogicalMessageContext>
{
    public override Task Invoke(IOutgoingLogicalMessageContext context, Func<Task> next)
    {
        #region PropagateTenantIdOutgoing

        if (context.Extensions.TryGet("TenantId", out var tenant))
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
            : base(
                stepId: "PropagateOutgoingTenantId",
                behavior: typeof(PropagateOutgoingTenantIdBehavior),
                description: "Sets the tenant header from the context bag into the messages header.")
        {
        }
    }
}