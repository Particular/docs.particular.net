﻿namespace Core9.Audit
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus.Pipeline;

    #region AddAuditData
    public class CustomAuditDataBehavior : Behavior<IAuditContext>
    {
        public override Task Invoke(IAuditContext context, Func<Task> next)
        {
            context.AuditMetadata["myKey"] = "MyValue";
            return next();
        }
    }
    #endregion
}