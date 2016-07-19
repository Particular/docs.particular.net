using System;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

internal class PropagateTenantIdBehavior :
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
            : base("PropagateTenantId", typeof(PropagateTenantIdBehavior), "Sets the tenant header on outgoing messages.")
        {
            InsertBefore(WellKnownStep.SerializeMessage);
        }
    }
}