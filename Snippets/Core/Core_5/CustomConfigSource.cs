namespace Core5
{
    using System.Configuration;
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    class CustomConfigSource
    {
        CustomConfigSource(BusConfiguration busConfiguration)
        {
            #region RegisterCustomConfigSource

            busConfiguration.CustomConfigurationSource(new MyCustomConfigurationSource());

            #endregion
        }

    }

    #region CustomConfigSource

    public class MyCustomConfigurationSource : IConfigurationSource
    {
        public T GetConfiguration<T>() where T : class, new()
        {
            // the part you are overriding
            if (typeof(T) == typeof(RijndaelEncryptionServiceConfig))
            {
                return new RijndaelEncryptionServiceConfig
                {
                    Key = "gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6"
                } as T;
            }
            // leaving the rest of the configuration as is:
            return ConfigurationManager.GetSection(typeof(T).Name) as T;
        }
    }

    #endregion

    #region CustomConfigProvider

    class CustomRijndaelEncryptionServiceConfigProvider :
        IProvideConfiguration<RijndaelEncryptionServiceConfig>
    {
        public RijndaelEncryptionServiceConfig GetConfiguration()
        {
            return new RijndaelEncryptionServiceConfig
            {
                Key = "gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6"
            };
        }
    }

    #endregion
}