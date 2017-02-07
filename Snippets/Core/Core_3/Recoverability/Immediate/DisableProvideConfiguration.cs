namespace Core3.Recoverability.Immediate
{
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region DisableImmediateRetriesProvideConfiguration

    class DisableProvideConfiguration :
        IProvideConfiguration<MsmqTransportConfig>
    {
        public MsmqTransportConfig GetConfiguration()
        {
            return new MsmqTransportConfig
            {
                MaxRetries = 0
            };
        }
    }

    #endregion
}