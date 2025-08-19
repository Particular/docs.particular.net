using System;
using NServiceBus;
using NServiceBus.Encryption.MessageProperty;

public static class EncryptionExtensions
{
    #region ConfigureEncryption

    public static void ConfigurationEncryption(this EndpointConfiguration endpointConfiguration)
    {
        var encryptionService = new AesEncryptionService(
            encryptionKeyIdentifier: "2015-10",
            key: Convert.FromBase64String("v96c7+e1aqFeNhMdZneHAQITEcgJu1z3ENSFUIr+FoM="));
        endpointConfiguration.EnableMessagePropertyEncryption(encryptionService,
            encryptedPropertyConvention: info => info.Name.StartsWith("Encrypted"));
    }

    #endregion
}