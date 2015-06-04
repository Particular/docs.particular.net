namespace Snippets4.Audit
{
    using System;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    public class ConfigureAuditUsingCode
    {

        #region ConfigureAuditUsingCode

        class ProvideAuditConfig : IProvideConfiguration<AuditConfig>
        {
            public AuditConfig GetConfiguration()
            {
                return new AuditConfig
                {
                    QueueName = "auditqueue@adminmachine",
                    OverrideTimeToBeReceived = TimeSpan.FromMinutes(10)
                };
            }
        }

        #endregion
    }
}
