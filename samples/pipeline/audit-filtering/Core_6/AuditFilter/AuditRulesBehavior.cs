using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region auditRulesBehavior
public class AuditRulesBehavior : Behavior<IIncomingLogicalMessageContext>
{
    public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        if (context.Message.MessageType == typeof(DoNotAuditThisMessage))
        {
            context.Extensions.Get<AuditFilterContext>().SkipAudit = true;
        }

        return next();
    }
}
#endregion