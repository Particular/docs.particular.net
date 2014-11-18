using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Encryption;

public class PropertyEncryption
{

    public void Simple()
    {
        #region EncryptionServiceSimpleV4
        Configure.With()
            .RijndaelEncryptionService();

        #endregion
    }
    public void Convention()
    {
        #region DefiningEncryptedPropertiesAsV4
        Configure.With()
            .DefiningEncryptedPropertiesAs(info => info.Name.EndsWith("EncryptedProperty"));
        #endregion
    }

    #region MessageForEncryptionConventionV4
    public class Message1 : IMessage
    {
        public string MyEncryptedProperty { get; set; }
    }
    #endregion
    #region MessageWithEncryptedPropertyV4
    public class MyMessage2 : IMessage
    {
        public WireEncryptedString MyEncryptedProperty { get; set; }
    }
    #endregion

    #region EncryptionFromIProvideConfigurationV4

    public class ConfigureEncryption : IProvideConfiguration<RijndaelEncryptionServiceConfig>
    {
        public RijndaelEncryptionServiceConfig GetConfiguration()
        {
            return new RijndaelEncryptionServiceConfig
            {
                Key = "gdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6",
                ExpiredKeys = new RijndaelExpiredKeyCollection
                		{
                    		new RijndaelExpiredKey { Key = "abDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6" },
		                    new RijndaelExpiredKey { Key = "cdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6" }
						}
            };
        }
    }
    #endregion

    public void FromCustomIEncryptionService()
    {
        #region EncryptionFromIEncryptionServiceV4
        //where MyCustomEncryptionService implements IEncryptionService 
        Configure.With()
            .Configurer.RegisterSingleton<IEncryptionService>(new MyCustomEncryptionService());
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
