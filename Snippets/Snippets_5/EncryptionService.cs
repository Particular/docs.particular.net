using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Encryption;

public class EncryptionService
{
    public void Simple()
    {
        #region RijndaelEncryptionServiceSimpleV5

        Configure.With(builder => builder.RijndaelEncryptionService());

        #endregion
    }

    #region RijndaelEncryptionServiceFromAppConfigV5

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

    #endregion

    #region RijndaelEncryptionFromCustomIProvideConfigurationV5

    public void FromCustomIProvideConfiguration()
    {
        Configure.With(builder => builder.RijndaelEncryptionService());
    }

    public class ConfigureEncryption : IProvideConfiguration<RijndaelEncryptionServiceConfig>
    {
        public RijndaelEncryptionServiceConfig GetConfiguration()
        {
            return new RijndaelEncryptionServiceConfig
            {
                Key = "gdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6"
            };
        }
    }

    #endregion

    public void FromCustomIEncryptionService()
    {
        #region FromCustomIEncryptionServiceV5

        Configure.With(configBuilder => configBuilder.RegisterEncryptionService(builder => new MyCustomEncryptionService()));

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