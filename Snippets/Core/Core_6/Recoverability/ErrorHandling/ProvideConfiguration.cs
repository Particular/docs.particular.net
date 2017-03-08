namespace Core6.Recoverability.ErrorHandling
{
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #pragma warning disable CS0618
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
    #pragma warning restore CS0618
}


