namespace Core6.Recoverability.ErrorHandling
{
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region ErrorQueueConfigurationProvider

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


