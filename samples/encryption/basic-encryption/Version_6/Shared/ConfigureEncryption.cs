using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

#region ConfigureEncryption
public class ConfigureEncryption :
    IProvideConfiguration<RijndaelEncryptionServiceConfig>
{
    public RijndaelEncryptionServiceConfig GetConfiguration()
    {
        return new RijndaelEncryptionServiceConfig
               {
                   Key = "gdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6",
                   KeyIdentifier = "2015-10",
                   KeyFormat = KeyFormat.Base64
        };
    }
}
#endregion