using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Encryption;

public class EncryptionService
{

    public void Simple()
    {
        #region RijndaelEncryptionServiceSimpleV4
        Configure.With()
            .RijndaelEncryptionService();
        #endregion
    }

    #region RijndaelEncryptionServiceFromAppConfigV4
    // Add this to the app.config
    /*
<configSections>
<section name="RijndaelEncryptionServiceConfig" 
         type="NServiceBus.Config.RijndaelEncryptionServiceConfig, NServiceBus.Core"/>
</configSections>
<RijndaelEncryptionServiceConfig Key="gdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6"/>
     */
    public void FromAppConfig()
    {
        Configure.With()
            .RijndaelEncryptionService();
    }
    #endregion

    #region RijndaelEncryptionFromCustomIProvideConfigurationV4
    public void FromCustomIProvideConfiguration()
    {
        Configure.With()
            .RijndaelEncryptionService();
    }

    public class ConfigureEncryption : IProvideConfiguration<RijndaelEncryptionServiceConfig>
    {
        public RijndaelEncryptionServiceConfig GetConfiguration()
        {
            return new RijndaelEncryptionServiceConfig { Key = "gdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6" };
        }
    }
    #endregion

    public void FromCustomIEncryptionService()
    {
        #region FromCustomIEncryptionServiceV4
        Configure.With()
            .Configurer.ConfigureComponent<IEncryptionService>(() => new MyCustomEncryptionService(), DependencyLifecycle.SingleInstance);
        #endregion
    }

    public class MyCustomEncryptionService : IEncryptionService
    {
        public EncryptedValue Encrypt(string value)
        {
            return null;
        }

        public string Decrypt(EncryptedValue encryptedValue)
        {
            return null;
        }
    }


}
