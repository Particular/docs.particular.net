namespace Core4.Audit
{
    using System;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region AuditProvideConfiguration

    class ProvideConfiguration :
        IProvideConfiguration<AuditConfig>
    {
        public AuditConfig GetConfiguration()
        {
            return new AuditConfig
            {
                QueueName = "targetAuditQueue",
                OverrideTimeToBeReceived = TimeSpan.FromMinutes(10)
            };
        }
    }

    #endregion
}
