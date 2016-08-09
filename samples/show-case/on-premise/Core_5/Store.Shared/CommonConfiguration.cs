using NServiceBus;

public static class CommonConfiguration
{
    public static void ApplyCommonConfiguration(this BusConfiguration busConfiguration)
    {
        busConfiguration.UseTransport<MsmqTransport>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.RijndaelEncryptionService();
        busConfiguration.EnableInstallers();
    }
}