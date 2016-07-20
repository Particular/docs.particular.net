using System;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

class PropagateTenantIdBehavior :
    IBehavior<OutgoingContext>
{
    public void Invoke(OutgoingContext context, Action next)
    {
        #region PropagateTenantId

        string tenant;
        if (!context.IncomingMessage.Headers.TryGetValue("TenantId", out tenant))
        {
            next();
            return;
        }
        context.OutgoingLogicalMessage.Headers["TenantId"] = tenant;
        next();

        #endregion
    }

    public class Registration :
        RegisterStep
    {
        public Registration()
            : base(
                stepId: "PropagateTenantId",
                behavior: typeof(PropagateTenantIdBehavior),
                description: "Sets the tenant header on outgoing messages.")
        {
            InsertBefore(WellKnownStep.SerializeMessage);
        }
    }
}