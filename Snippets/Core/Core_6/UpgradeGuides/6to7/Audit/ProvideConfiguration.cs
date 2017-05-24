#pragma warning disable 618
namespace Core_6.UpgradeGuides._6to7.ProvideConfiguration.Audit
{
    using System;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region 6to7AuditProvideConfiguration

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
