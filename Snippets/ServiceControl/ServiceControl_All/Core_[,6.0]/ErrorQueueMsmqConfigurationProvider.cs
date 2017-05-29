#pragma warning disable 618
namespace ServiceControl.ConfigureEndpoints
{
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region ErrorQueueRemoteMachineConfigurationProvider

    class ProvideConfiguration :
        IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
    {
        public MessageForwardingInCaseOfFaultConfig GetConfiguration()
        {
            return new MessageForwardingInCaseOfFaultConfig
            {
                ErrorQueue = "error@machinename"
            };
        }
    }

    #endregion
}