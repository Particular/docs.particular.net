using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

class AuditMetadata
{
    #region core-8to9-audit-metadata
    public class MyAuditDataBehavior : Behavior<IAuditContext>
    {
        public override Task Invoke(IAuditContext context, Func<Task> next)
        {
            context.AuditMetadata["myKey"] = "MyValue";
            return next();
        }
    }
    #endregion
}