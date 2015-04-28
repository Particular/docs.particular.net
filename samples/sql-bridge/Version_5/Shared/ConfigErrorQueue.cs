using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

namespace Shared
{
    public class ConfigErrorQueue : IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
    {

        public static string errorQueue = "error";

        public MessageForwardingInCaseOfFaultConfig GetConfiguration()
        {
            return new MessageForwardingInCaseOfFaultConfig
                   {
                       ErrorQueue = errorQueue
                   };
        }
    }
}