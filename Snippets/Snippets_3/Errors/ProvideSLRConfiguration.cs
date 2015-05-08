namespace Snippets_4.Errors
{
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;
    using System;
    using System.Configuration;

    #region SlrConfiguration
    public class ProvideSLRConfiguration : IConfigurationSource
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

            // To look at the app.config for other sections that's not defined in this method, otherwise return null.
            return ConfigurationManager.GetSection(typeof(T).Name) as T;
        }
    }
    #endregion

    public class InjectProvideSLRConfiguration
    {
        public void Foo()
        {
            #region UseCustomConfigurationSourceForFLR
            Configure.With()
                .CustomConfigurationSource(new ProvideSLRConfiguration());
            #endregion
        }
    }

}
