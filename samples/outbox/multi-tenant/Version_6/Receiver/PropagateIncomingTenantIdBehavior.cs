using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

internal class PropagateIncomingTenantIdBehavior : Behavior<IIncomingLogicalMessageContext>
{
    public override async Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        #region PropagateTenantId
        
        string tenant;
        if (context.MessageHeaders.TryGetValue("TenantId", out tenant))
        {
            context.Extensions.Set("TenantId", tenant);
        }
        await next().ConfigureAwait(false);

        #endregion
    }

    public class Registration : RegisterStep
    {
        public Registration()
            : base("PropagateIncomingTenantId", typeof(PropagateIncomingTenantIdBehavior), "Sets the tenant header on outgoing messages's context bag.")
        {
        }
    }
}