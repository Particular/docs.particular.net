namespace Snippets_4.Errors
{
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;
    using System;
    using System.Configuration;

    #region FlrConfiguration

    // Create a configuration source 
    public class ProvideFLRConfiguration : IConfigurationSource
    {
        public T GetConfiguration<T>() where T : class, new()
        {

            //To Provide FLR Config
            if (typeof(T) == typeof(MsmqTransportConfig))
            {
                var flrConfig = new MsmqTransportConfig()
                {
                    MaxRetries = 2
                };

                return flrConfig as T;
            }


            // You can also override other sections such as MessageForwardingInCaseOfFaultConfig, 
            // SecondLevelRetriesConfig, MsmqSubscriptionStorageConfig,
            // DBSubscriptionStorageConfig and UnicastBusConfig in a similar fashion as above
            // in the same method.

            // To look at the app.config for other sections that's not defined in this method, otherwise return null.
            return ConfigurationManager.GetSection(typeof(T).Name) as T;
        }
    }
    #endregion

    #region ErrorQueueConfiguration
    public class ProvideErrorQueueConfiguration : IConfigurationSource
    {
        public T GetConfiguration<T>() where T : class, new()
        {

            if (typeof(T) == typeof(MessageForwardingInCaseOfFaultConfig))
            {
                var errorConfig = new MessageForwardingInCaseOfFaultConfig()
                {
                    ErrorQueue = "error"
                };

                return errorConfig as T;
            }

            // You can also override other sections such as MsmqTransportConfig, 
            // SecondLevelRetriesConfig, MsmqSubscriptionStorageConfig,
            // DBSubscriptionStorageConfig and UnicastBusConfig in a similar fashion as above
            // in the same method.

            // To look at the app.config for other sections that's not defined in this method, otherwise return null.
            return ConfigurationManager.GetSection(typeof(T).Name) as T;
        }
    }
    #endregion

    #region SlrConfiguration
    public class ProvideSLRConfiguration : IConfigurationSource
    {
        public T GetConfiguration<T>() where T : class, new()
        {
            // To provide SLR Config
            if (typeof(T) == typeof(SecondLevelRetriesConfig))
            {
                var slrConfig = new SecondLevelRetriesConfig()
                {
                    Enabled = true,
                    NumberOfRetries = 2, 
                    TimeIncrease = TimeSpan.FromSeconds(10)
                };

                return slrConfig as T;
            }

            // You can also override other sections such as MsmqTransportConfig, 
            // MessageForwardingInCaseOfFaultConfig, MsmqSubscriptionStorageConfig,
            // DBSubscriptionStorageConfig and UnicastBusConfig in a similar fashion as above
            // in the same method.

            // To look at the app.config for other sections that's not defined in this method, otherwise return null.
            return ConfigurationManager.GetSection(typeof(T).Name) as T;
        }
    }
    #endregion

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, IWantCustomInitialization
    {
        public void Init()
        {
            #region UseCustomConfigurationSourceForErrorQueueConfig
            Configure.With()
                .CustomConfigurationSource(new ProvideErrorQueueConfiguration()); 
            #endregion

            #region UseCustomConfigurationSourceForSLR
            Configure.With()
                .CustomConfigurationSource(new ProvideSLRConfiguration());
            #endregion


            #region UseCustomConfigurationSourceForFLR
            Configure.With()
                .CustomConfigurationSource(new ProvideFLRConfiguration());
            #endregion
        }
    }

}
