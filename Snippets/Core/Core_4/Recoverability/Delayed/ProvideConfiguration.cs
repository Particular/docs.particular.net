namespace Core4.Recoverability.Delayed
{
    using System;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region DelayedRetriesProvideConfiguration

    class ProvideConfiguration :
        IProvideConfiguration<SecondLevelRetriesConfig>
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