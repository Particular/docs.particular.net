using NServiceBus;
using NServiceBus.Persistence;

class ConfiguringInMemory
{
    public void Version_5_0()
    {
#pragma warning disable 618

        #region ConfiguringInMemory 5.0

        BusConfiguration config = new BusConfiguration();

        config.UsePersistence<InMemoryPersistence>()
            .For(
                Storage.Sagas,
                Storage.Subscriptions,
                Storage.Timeouts,
                Storage.Outbox,
                Storage.GatewayDeduplication);

        #endregion

#pragma warning restore 618

    }

    public void Version_5_2()
    {
        #region ConfiguringInMemory 5.2

        BusConfiguration config = new BusConfiguration();

        config.UsePersistence<InMemoryPersistence, StorageType.Sagas>();
        config.UsePersistence<InMemoryPersistence, StorageType.Subscriptions>();
        config.UsePersistence<InMemoryPersistence, StorageType.Timeouts>();
        config.UsePersistence<InMemoryPersistence, StorageType.Outbox>();
        config.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();

        #endregion
    }
}