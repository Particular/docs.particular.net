namespace Core5.Recoverability.Immediate
{
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region DisableImmediateRetriesProvideConfiguration

    class DisableProvideConfiguration :
        IProvideConfiguration<TransportConfig>
    {
        public TransportConfig GetConfiguration()
        {
            return new TransportConfig
            {
                MaxRetries = 0
            };
        }
    }

    #endregion
}