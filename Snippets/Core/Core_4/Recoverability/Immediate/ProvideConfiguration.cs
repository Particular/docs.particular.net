namespace Core4.Recoverability.Immediate
{
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region FlrProvideConfiguration

    class ProvideConfiguration :
        IProvideConfiguration<TransportConfig>
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
}