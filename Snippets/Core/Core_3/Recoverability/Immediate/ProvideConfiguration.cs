namespace Core3.Recoverability.Immediate
{
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region ImmediateRetriesProvideConfiguration

    class ProvideConfiguration :
        IProvideConfiguration<MsmqTransportConfig>
    {
        public MsmqTransportConfig GetConfiguration()
        {
            return new MsmqTransportConfig
            {
                MaxRetries = 2
            };
        }
    }

    #endregion
}