using System;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

#region audit
class ConfigAuditQueue : IProvideConfiguration<AuditConfig>
{
    public AuditConfig GetConfiguration()
    {
        return new AuditConfig
        {
            QueueName = "audit",
        };
    }
}
#endregion