using System;

#region ConfigureEncryption
using NServiceBus;
using ConfigureEncryption = NServiceBus.Encryption.MessageProperty.ConfigureRijndaelEncryptionService;

public static class EncryptionExtensions
{
    public static void ConfigurationEncryption(this EndpointConfiguration endpointConfiguration)
    {
        var encryptionKey = Convert.FromBase64String("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6");
        ConfigureEncryption.RijndaelEncryptionService(endpointConfiguration, "2015-10", encryptionKey);
    }
}
#endregion