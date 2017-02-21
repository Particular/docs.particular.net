using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

class ConfigureAuditQueue :
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