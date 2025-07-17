using NServiceBus.Pipeline;

#region StoreTenantId
public class StoreTenantIdBehavior :
    Behavior<IIncomingLogicalMessageContext>
{
    public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        if (context.MessageHeaders.TryGetValue("tenant_id", out var tenant))
        {
            context.Extensions.Set("TenantId", tenant);
        }
        return next();

    }
}
#endregion