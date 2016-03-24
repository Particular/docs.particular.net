using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

internal class PropagateOutgoingTenantIdBehavior : Behavior<IOutgoingLogicalMessageContext>
{
    public override async Task Invoke(IOutgoingLogicalMessageContext context, Func<Task> next)
    {
        string tenant;
        if (context.Extensions.TryGet("TenantId", out tenant))
        {
            context.Headers["TenantId"] = tenant;
        }
        await next().ConfigureAwait(false);
    }

    public class Registration : RegisterStep
    {
        public Registration()
            : base("PropagateOutgoingTenantId", typeof(PropagateOutgoingTenantIdBehavior), "Sets the tenant header from the context bag into the messages header.")
        {
        }
    }
}