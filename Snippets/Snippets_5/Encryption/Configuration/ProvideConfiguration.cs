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
                ExpiredKeys = new RijndaelExpiredKeyCollection
                {
                    new RijndaelExpiredKey
                    {
                        Key = "abDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6"
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
