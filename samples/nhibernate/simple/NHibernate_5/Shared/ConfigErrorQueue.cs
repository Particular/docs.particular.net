using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

namespace Shared
{
    class ConfigErrorQueue :
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
}