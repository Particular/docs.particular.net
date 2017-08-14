using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region auditFilterBehavior

public class AuditFilterBehavior :
    Behavior<IAuditContext>
{
    public override Task Invoke(IAuditContext context, Func<Task> next)
    {
        if (context.Extensions.TryGet(out var auditFilterContext) &&
            auditFilterContext.SkipAudit)
        {
            return Task.CompletedTask;
        }

        return next();
    }
}

#endregion