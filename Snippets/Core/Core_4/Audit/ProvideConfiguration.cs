namespace Core4.Audit
{
    using System;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region AuditProvideConfiguration

    class ProvideConfiguration : IProvideConfiguration<AuditConfig>
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