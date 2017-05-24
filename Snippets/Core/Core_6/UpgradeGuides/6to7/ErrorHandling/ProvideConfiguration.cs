#pragma warning disable 618
namespace Core_6.UpgradeGuides._6to7.ErrorHandling
{
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region 6to7ErrorQueueConfigurationProvider

    class ProvideConfiguration :
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

    #endregion
}


