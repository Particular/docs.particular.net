using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

class ConfigAuditQueue : IProvideConfiguration<UnicastBusConfig>
{
    public UnicastBusConfig GetConfiguration()
    {
        return new UnicastBusConfig 
        {
            ForwardReceivedMessagesTo = "audit"
        };
    }
}