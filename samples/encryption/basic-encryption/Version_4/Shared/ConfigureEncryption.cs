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
            Key = "do-not-use-this-encryption-key!!",
            KeyIdentifier = "2015-10",
        };
    }
}
#endregion