using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region auditRulesBehavior
public class AuditRulesBehavior :
    Behavior<IIncomingLogicalMessageContext>
{
    public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        if (context.Message.Instance is DoNotAuditThisMessage)
        {
            var auditFilterContext = context.Extensions.Get<AuditFilterContext>();
            auditFilterContext.SkipAudit = true;
        }

        return next();
    }
}
#endregion