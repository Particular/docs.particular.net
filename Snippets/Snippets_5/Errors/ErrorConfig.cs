namespace Snippets_4.Errors
{
    using NServiceBus;
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


    #region FlrConfiguration
    class ConfigureFirstLevelRetries : IProvideConfiguration<TransportConfig>
    {
        public TransportConfig GetConfiguration()
        {
            return new TransportConfig
            {
                MaxRetries = 2
            };
        }
    }
    #endregion

    #region SlrConfiguration
    class ConfigureSecondLevelRetries : IProvideConfiguration<SecondLevelRetriesConfig>
    {
        public SecondLevelRetriesConfig GetConfiguration()
        {
            return new SecondLevelRetriesConfig
            {
                Enabled = true,
                NumberOfRetries = 2,
                TimeIncrease = TimeSpan.FromSeconds(10)
            };
        }
    }
    #endregion
}
