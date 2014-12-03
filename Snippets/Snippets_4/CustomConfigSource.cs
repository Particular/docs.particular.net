using System.Configuration;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

public class CustomConfigSource
{
    public CustomConfigSource()
    {

        #region RegisterCustomConfigSource 4

        Configure.With()
            //this call needs to happen right after With()
            .CustomConfigurationSource(new MyCustomConfigurationSource()); 

        #endregion 
    }


    #region CustomConfigSource 4

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

    #region CustomConfigProvider 4

    class CustomRijndaelEncryptionServiceConfigProvider : IProvideConfiguration<RijndaelEncryptionServiceConfig>
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
