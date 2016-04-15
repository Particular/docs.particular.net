namespace Snippets5.Encryption.Configuration
{
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region EncryptionFromIProvideConfiguration

    public class ProvideConfiguration : IProvideConfiguration<RijndaelEncryptionServiceConfig>
    {
        public RijndaelEncryptionServiceConfig GetConfiguration()
        {
            return new RijndaelEncryptionServiceConfig
            {
                Key = "gdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6",
                KeyIdentifier = "2015-10",
                KeyFormat = KeyFormat.Base64,
                ExpiredKeys = new RijndaelExpiredKeyCollection
                {
                    new RijndaelExpiredKey
                    {
                        Key = "abDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6",
                        KeyIdentifier = "2015-09",
                        KeyFormat = KeyFormat.Base64
                    },
                    new RijndaelExpiredKey
                    {
                        Key = "cdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6"
                    }
                }
            };
        }
    }

    #endregion
}
