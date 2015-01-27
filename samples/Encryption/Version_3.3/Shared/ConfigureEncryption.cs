using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

public class ConfigureEncryption :
    IProvideConfiguration<RijndaelEncryptionServiceConfig>
{
    public RijndaelEncryptionServiceConfig GetConfiguration()
    {
        return new RijndaelEncryptionServiceConfig
               {
                   Key = "gdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6"
               };
    }
}