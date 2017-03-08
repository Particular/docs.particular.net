namespace Core6.Audit
{
    using System;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #pragma warning disable CS0618
    #region AuditProvideConfiguration

    class ProvideConfiguration :
        IProvideConfiguration<AuditConfig>
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
    #pragma warning restore CS0618
}
