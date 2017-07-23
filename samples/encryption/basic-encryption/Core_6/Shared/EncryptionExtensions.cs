using System;

#region ConfigureEncryption
using NServiceBus;

public static class EncryptionExtensions
{
    public static void ConfigurationEncryption(this EndpointConfiguration endpointConfiguration)
    {
        var encryptionKey = Convert.FromBase64String("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6");
        #pragma warning disable 618
        // Property Encryption has been moved to an external nuget package: NServiceBus.Encryption.MessageProperty
        // Old Encryption APIs marked obsolete in 6.2 of NServiceBus
        endpointConfiguration.RijndaelEncryptionService("2015-10", encryptionKey);
    }
}

#endregion