using System;
using NServiceBus;
using NServiceBus.Encryption.MessageProperty;

public static class EncryptionExtensions
{
    #region ConfigureEncryption

    public static void ConfigurationEncryption(this EndpointConfiguration endpointConfiguration)
    {
        var encryptionService = new RijndaelEncryptionService(
            encryptionKeyIdentifier: "2015-10",
            key: Convert.FromBase64String("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6"));
        endpointConfiguration.EnableMessagePropertyEncryption(encryptionService,
            encryptedPropertyConvention: info =>
            {
                return info.Name.StartsWith("Encrypted");
            });
    }

    #endregion
}