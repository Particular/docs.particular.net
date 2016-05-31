using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region auditFilterContextBehavior
public class AuditFilterContextBehavior : Behavior<IIncomingPhysicalMessageContext>
{
    public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
    {
        context.Extensions.Set(new AuditFilterContext { SkipAudit = false });

        return next();
    }
}
#endregion

class AuditFilterContext
{
    public bool SkipAudit { get; set; }
}