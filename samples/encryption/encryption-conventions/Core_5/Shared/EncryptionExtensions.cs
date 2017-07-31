using System;

using NServiceBus;

public static class EncryptionExtensions
{
    public static void ConfigurationEncryption(this BusConfiguration busConfiguration)
    {
        #region ConfigureEncryption
        var conventions = busConfiguration.Conventions();
        
        var encryptionKey = Convert.FromBase64String("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6");
        busConfiguration.RijndaelEncryptionService("2015-10", encryptionKey);
        
        conventions.DefiningEncryptedPropertiesAs(
                property =>
                        {
                            return property.Name.StartsWith("Encrypted");
                        });
        #endregion
    }
}