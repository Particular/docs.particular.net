using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

#region ErrorQueueConfiguration
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
#endregion



