using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

class ConfigureErrorQueue :
    IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
{
    public MessageForwardingInCaseOfFaultConfig GetConfiguration()
    {
        return new MessageForwardingInCaseOfFaultConfig
        {
            ErrorQueue = "error"
        };
    }
}