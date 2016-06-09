using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

class ProvideAuditConfig : IProvideConfiguration<AuditConfig>
{
    public AuditConfig GetConfiguration()
    {
        return new AuditConfig
        {
            QueueName = "auditqueue@adminmachine",
        };
    }
}