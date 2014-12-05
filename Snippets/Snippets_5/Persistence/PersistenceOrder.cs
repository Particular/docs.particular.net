using NServiceBus;
using NServiceBus.Persistence;

class PersistenceOrder
{
    void Setup_5_0()
    {
        #region PersistenceOrder_Correct 5.0
        var config = new BusConfiguration();

        config.UsePersistence<RavenDBPersistence>();
            
        config.UsePersistence<NHibernatePersistence>()
            .For(Storage.Outbox);

        config.UsePersistence<InMemoryPersistence>()
            .For(Storage.GatewayDeduplication);
        #endregion
    }

    void Setup_5_2()
    {
        #region PersistenceOrder_Correct 5.2
        var config = new BusConfiguration();

        config.UsePersistence<RavenDBPersistence>();

        config.UsePersistence<NHibernatePersistence, StorageType.Outbox>();

        config.UsePersistence<InMemoryPersistence, Storage.GatewayDeduplication>();
        #endregion
    }

    void Setup3_5_0()
    {
        #region PersistenceOrder_Explicit 5.0
        var config = new BusConfiguration();

        config.UsePersistence<NHibernatePersistence>()
            .For(Storage.Outbox);

        config.UsePersistence<InMemoryPersistence>()
            .For(Storage.GatewayDeduplication);

        config.UsePersistence<RavenDBPersistence>()
            .For(Storage.Sagas, Storage.Subscriptions, Storage.Timeouts);
        #endregion
    }

    void Setup3_5_2()
    {
        #region PersistenceOrder_Explicit 5.2
        var config = new BusConfiguration();

        config.UsePersistence<NHibernatePersistence, StorageType.Outbox>();

        config.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();

        config.UsePersistence<RavenDBPersistence, StorageType.Sagas>();
        config.UsePersistence<RavenDBPersistence, StorageType.Subscriptions>();
        config.UsePersistence<RavenDBPersistence, StorageType.Timeouts>();
        #endregion
    }

    void Setup2_5_0()
    {
        #region PersistenceOrder_Incorrect 5.0
        var config = new BusConfiguration();

        config.UsePersistence<NHibernatePersistence>()
            .For(Storage.Outbox);

        config.UsePersistence<InMemoryPersistence>()
            .For(Storage.GatewayDeduplication);

        // This one will override the above settings!
        config.UsePersistence<RavenDBPersistence>();
        #endregion
    }

    void Setup2_5_2()
    {
        #region PersistenceOrder_Incorrect 5.2
        var config = new BusConfiguration();

        config.UsePersistence<NHibernatePersistence, StorageType.Outbox>();

        config.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();
            
        // This one will override the above settings!
        config.UsePersistence<RavenDBPersistence>();
        #endregion
    }
}
