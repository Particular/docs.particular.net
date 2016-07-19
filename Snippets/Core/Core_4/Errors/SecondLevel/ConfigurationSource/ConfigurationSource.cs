namespace Core4.Errors.SecondLevel.ConfigurationSource
{
    using System;
    using System.Configuration;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region SlrConfigurationSource
    public class ConfigurationSource :
        IConfigurationSource
    {
        public T GetConfiguration<T>() where T : class, new()
        {
            // To provide SLR Config
            if (typeof(T) == typeof(SecondLevelRetriesConfig))
            {
                var slrConfig = new SecondLevelRetriesConfig
                {
                    Enabled = true,
                    NumberOfRetries = 2,
                    TimeIncrease = TimeSpan.FromSeconds(10)
                };

                return slrConfig as T;
            }

            // To in app.config for other sections not defined in this method, otherwise return null.
            return ConfigurationManager.GetSection(typeof(T).Name) as T;
        }
    }
    #endregion
}
