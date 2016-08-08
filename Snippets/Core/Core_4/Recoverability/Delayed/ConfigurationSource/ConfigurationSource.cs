namespace Core4.Recoverability.Delayed.ConfigurationSource
{
    using System;
    using System.Configuration;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region DelayedRetriesConfigurationSource
    public class ConfigurationSource :
        IConfigurationSource
    {
        public T GetConfiguration<T>() where T : class, new()
        {
            if (typeof(T) == typeof(SecondLevelRetriesConfig))
            {
                var config = new SecondLevelRetriesConfig
                {
                    Enabled = true,
                    NumberOfRetries = 2,
                    TimeIncrease = TimeSpan.FromSeconds(10)
                };

                return config as T;
            }

            // Respect app.config for other sections not defined in this method
            return ConfigurationManager.GetSection(typeof(T).Name) as T;
        }
    }
    #endregion
}
