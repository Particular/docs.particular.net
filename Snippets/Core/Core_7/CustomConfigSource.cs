﻿namespace Core6
{
    using System.Configuration;
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    class CustomConfigSource
    {
        CustomConfigSource(EndpointConfiguration endpointConfiguration)
        {
            #region RegisterCustomConfigSource

            endpointConfiguration.CustomConfigurationSource(new MyCustomConfigurationSource());

            #endregion
        }

    }

#pragma warning disable 618
    #region CustomConfigSource

    public class MyCustomConfigurationSource :
        IConfigurationSource
    {
        public T GetConfiguration<T>() where T : class, new()
        {
#pragma warning disable 618
            if (typeof(T) == typeof(RijndaelEncryptionServiceConfig))
            {
                var config = new RijndaelEncryptionServiceConfig
                {
#pragma warning restore 618
                    Key = "gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6"
                };
                return config as T;
            }
            // leaving the rest of the configuration as is:
            return ConfigurationManager.GetSection(typeof(T).Name) as T;
        }
    }

    #endregion
#pragma warning restore 618

#pragma warning disable 618
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
#pragma warning restore 618
}