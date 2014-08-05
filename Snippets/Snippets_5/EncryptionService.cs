using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Encryption;

public class EncryptionService
{
    public void Simple()
    {
        // start code RijndaelEncryptionServiceSimpleV5
        Configure.With(builder => builder.RijndaelEncryptionService());
        // end code RijndaelEncryptionServiceSimpleV5
    }

    // start code RijndaelEncryptionServiceFromAppConfigV5
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
        Configure.With(builder => builder.RijndaelEncryptionService());
    }
    // end code RijndaelEncryptionServiceFromAppConfigV5

    // start code RijndaelEncryptionFromCustomIProvideConfigurationV5
    public void FromCustomIProvideConfiguration()
    {
        Configure.With(builder => builder.RijndaelEncryptionService());
    }

    public class ConfigureEncryption : IProvideConfiguration<RijndaelEncryptionServiceConfig>
    {
        public RijndaelEncryptionServiceConfig GetConfiguration()
        {
            return new RijndaelEncryptionServiceConfig { Key = "gdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6" };
        }
    }
    // end code RijndaelEncryptionFromCustomIProvideConfigurationV5

    // start code RijndaelEncryptionFromCustomEncryptionServiceV5
    public void FromCustomIEncryptionService()
    {
        Configure.With(configBuilder => configBuilder.RegisterEncryptionService(builder => new MyCustomEncryptionService()));
    }
    // end code RijndaelEncryptionFromCustomEncryptionServiceV5

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
