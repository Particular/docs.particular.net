namespace Core8.Audit
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus.Pipeline;

    #region AddAuditData
    public class CustomAuditDataBehavior : Behavior<IAuditContext>
    {
        public override Task Invoke(IAuditContext context, Func<Task> next)
        {
            context.AddAuditData("myKey", "myValue");
            return next();
        }
    }
    #endregion
}