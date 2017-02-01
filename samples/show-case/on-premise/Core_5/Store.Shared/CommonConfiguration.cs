using System;
using NServiceBus;

public static class CommonConfiguration
{
    public static void ApplyCommonConfiguration(this BusConfiguration busConfiguration)
    {
        busConfiguration.UseTransport<MsmqTransport>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        var encryptionKey = Convert.FromBase64String("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6");
        busConfiguration.RijndaelEncryptionService("2015-10", encryptionKey);
        busConfiguration.EnableInstallers();
    }
}