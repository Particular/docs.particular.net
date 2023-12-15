using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;
class AuditMetadataOld
{
#pragma warning disable CS0618 // Type or member is obsolete
    #region core-8to9-audit-metadata-old
    public class MyAuditDataBehavior : Behavior<IAuditContext>
    {
        public override Task Invoke(IAuditContext context, Func<Task> next)
        {
            context.AddAuditData("myKey","MyValue");
            return next();
        }
    }
    #endregion
#pragma warning restore CS0618 // Type or member is obsolete
}

class AuditMetadataNew
{
    #region core-8to9-audit-metadata-new
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
