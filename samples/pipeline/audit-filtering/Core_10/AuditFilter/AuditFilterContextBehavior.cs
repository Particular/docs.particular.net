using NServiceBus.Pipeline;

#region auditFilterContextBehavior
public class AuditFilterContextBehavior : Behavior<IIncomingPhysicalMessageContext>
{
    public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
    {
        var auditFilterContext = new AuditFilterContext
        {
            SkipAudit = false
        };

        context.Extensions.Set(auditFilterContext);

        return next();
    }
}
#endregion