using System;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

class ConfigErrorQueue : IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
{
    public MessageForwardingInCaseOfFaultConfig GetConfiguration()
    {
        return new MessageForwardingInCaseOfFaultConfig
               {
                   ErrorQueue = "error"
               };
    }
}
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