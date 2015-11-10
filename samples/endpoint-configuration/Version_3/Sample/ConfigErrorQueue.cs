using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

#region error
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