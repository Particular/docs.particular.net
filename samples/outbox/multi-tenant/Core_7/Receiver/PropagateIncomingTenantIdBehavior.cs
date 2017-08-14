using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

class PropagateIncomingTenantIdBehavior :
    Behavior<IIncomingLogicalMessageContext>
{
    public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        #region PropagateTenantId

        if (context.MessageHeaders.TryGetValue("TenantId", out var tenant))
        {
            context.Extensions.Set("TenantId", tenant);
        }
        return next();

        #endregion
    }

    public class Registration :
        RegisterStep
    {
        public Registration()
            : base(
                stepId: "PropagateIncomingTenantId",
                behavior: typeof(PropagateIncomingTenantIdBehavior),
                description: "Sets the tenant header on outgoing messages's context bag.")
        {
        }
    }
}