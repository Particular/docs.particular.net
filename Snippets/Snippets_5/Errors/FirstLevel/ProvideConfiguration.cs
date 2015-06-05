namespace Snippets5.Errors.FirstLevel
{
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region FlrProvideConfiguration

    class ProvideConfiguration : IProvideConfiguration<TransportConfig>
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