namespace Snippets5.Errors.SecondLevel
{
    using System;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region SlrProvideConfiguration

    class ProvideConfiguration : IProvideConfiguration<SecondLevelRetriesConfig>
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
