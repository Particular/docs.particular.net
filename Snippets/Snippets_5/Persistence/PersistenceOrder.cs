using NServiceBus;
using NServiceBus.Persistence;

class PersistenceOrder
{
    void Setup_5_0()
    {
#pragma warning disable 618

        #region PersistenceOrder_Correct 5.0
        BusConfiguration config = new BusConfiguration();

        config.UsePersistence<RavenDBPersistence>();
            
        config.UsePersistence<NHibernatePersistence>()
            .For(Storage.Outbox);

        config.UsePersistence<InMemoryPersistence>()
            .For(Storage.GatewayDeduplication);
        #endregion
#pragma warning restore 618
    }

    void Setup_5_2()
    {
        #region PersistenceOrder_Correct 5.2
        BusConfiguration config = new BusConfiguration();

        config.UsePersistence<RavenDBPersistence>();

        config.UsePersistence<NHibernatePersistence, StorageType.Outbox>();

        config.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();

        #endregion
    }

    void Setup3_5_0()
    {
#pragma warning disable 618

        #region PersistenceOrder_Explicit 5.0
        BusConfiguration config = new BusConfiguration();

        config.UsePersistence<NHibernatePersistence>()
            .For(Storage.Outbox);

        config.UsePersistence<InMemoryPersistence>()
            .For(Storage.GatewayDeduplication);

        config.UsePersistence<RavenDBPersistence>()
            .For(Storage.Sagas, Storage.Subscriptions, Storage.Timeouts);
        #endregion
#pragma warning restore 618

    }

    void Setup3_5_2()
    {
        #region PersistenceOrder_Explicit 5.2
        BusConfiguration config = new BusConfiguration();

        config.UsePersistence<NHibernatePersistence, StorageType.Outbox>();

        config.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();

        config.UsePersistence<RavenDBPersistence, StorageType.Sagas>();
        config.UsePersistence<RavenDBPersistence, StorageType.Subscriptions>();
        config.UsePersistence<RavenDBPersistence, StorageType.Timeouts>();
        #endregion
    }

    void Setup2_5_0()
    {
#pragma warning disable 618

        #region PersistenceOrder_Incorrect 5.0
        BusConfiguration config = new BusConfiguration();

        config.UsePersistence<NHibernatePersistence>()
            .For(Storage.Outbox);

        config.UsePersistence<InMemoryPersistence>()
            .For(Storage.GatewayDeduplication);

        // This one will override the above settings!
        config.UsePersistence<RavenDBPersistence>();
        #endregion
#pragma warning restore 618
    }

    void Setup2_5_2()
    {
        #region PersistenceOrder_Incorrect 5.2
        BusConfiguration config = new BusConfiguration();

        config.UsePersistence<NHibernatePersistence, StorageType.Outbox>();

        config.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();
            
        // This one will override the above settings!
        config.UsePersistence<RavenDBPersistence>();
        #endregion
    }
}
