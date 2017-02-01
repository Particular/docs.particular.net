using System;
using NServiceBus;

#region ConfigureEncryption
public static class EncryptionExtensions
{
    public static void ConfigurationEncryption(this BusConfiguration busConfiguration)
    {
        var encryptionKey = Convert.FromBase64String("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6");
        busConfiguration.RijndaelEncryptionService("2015-10", encryptionKey);
    }
}
#endregion