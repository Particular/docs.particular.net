using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Encryption;

public class PropertyEncryption
{
    public void Simple()
    {
        #region EncryptionServiceSimple

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.RijndaelEncryptionService();

        #endregion
    }
    public void Convention()
    {
        #region DefiningEncryptedPropertiesAs
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.Conventions()
            .DefiningEncryptedPropertiesAs(info => info.Name.EndsWith("EncryptedProperty"));
        #endregion
    }

    #region MessageForEncryptionConvention
    public class Message1:IMessage
    {
        public string MyEncryptedProperty { get; set; }
    }
    #endregion
    #region MessageWithEncryptedProperty
    public class MyMessage2 : IMessage
    {
        public WireEncryptedString MyEncryptedProperty { get; set; }
    }
    #endregion

    public void FromFluentAPI()
    {
        #region EncryptionFromFluentAPI

        BusConfiguration busConfiguration = new BusConfiguration();
        string encryptionKey = "gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6";
        List<string> expiredKeys = new List<string>
                          {
                              "abDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6",
                              "cdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6"
                          };
        busConfiguration.RijndaelEncryptionService(encryptionKey, expiredKeys);

        #endregion
    }

    #region EncryptionFromIProvideConfiguration

    public class ConfigureEncryption : IProvideConfiguration<RijndaelEncryptionServiceConfig>
    {
        public RijndaelEncryptionServiceConfig GetConfiguration()
        {
            return new RijndaelEncryptionServiceConfig
                   {
                       Key = "gdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6",
                       ExpiredKeys = new RijndaelExpiredKeyCollection
                                     {
                                         new RijndaelExpiredKey{Key = "abDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6"},
                                         new RijndaelExpiredKey{Key = "cdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6"}
                                     }
                   };
        }
    }

    #endregion

    public void FromCustomIEncryptionService()
    {
        #region EncryptionFromIEncryptionService

        BusConfiguration busConfiguration = new BusConfiguration();
        //where MyCustomEncryptionService implements IEncryptionService 
        busConfiguration.RegisterEncryptionService(b => new MyCustomEncryptionService());

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