using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

class ConfigAuditQueue :
    IProvideConfiguration<AuditConfig>
{
    public AuditConfig GetConfiguration()
    {
        return new AuditConfig
        {
            QueueName = "audit"
        };
    }
}