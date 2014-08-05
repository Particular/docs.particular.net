using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Encryption;

public class EncryptionService
{

    public void Simple()
    {
        // start code RijndaelEncryptionServiceSimpleV4
        Configure.With()
            .RijndaelEncryptionService();
        // end code RijndaelEncryptionServiceSimpleV4
    }

    // start code RijndaelEncryptionServiceFromAppConfigV4
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
    // end code RijndaelEncryptionServiceFromAppConfigV4

    // start code RijndaelEncryptionFromCustomIProvideConfigurationV4
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
    // end code RijndaelEncryptionFromCustomIProvideConfigurationV4

    public void FromCustomIEncryptionService()
    {
        // start code RijndaelEncryptionFromCustomEncryptionServiceV4
        Configure.With()
            .Configurer.ConfigureComponent<IEncryptionService>(() => new MyCustomEncryptionService(), DependencyLifecycle.SingleInstance);
        // end code RijndaelEncryptionFromCustomEncryptionServiceV4
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
