using NServiceBus;
using NServiceBus.Persistence;

class PersistenceOrder
{
    void Setup_5_0()
    {
#pragma warning disable 618

        #region PersistenceOrder_Correct 5.0
        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.UsePersistence<RavenDBPersistence>();
            
        busConfiguration.UsePersistence<NHibernatePersistence>()
            .For(Storage.Outbox);

        busConfiguration.UsePersistence<InMemoryPersistence>()
            .For(Storage.GatewayDeduplication);
        #endregion
#pragma warning restore 618
    }

    void Setup_5_2()
    {
        #region PersistenceOrder_Correct 5.2
        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.UsePersistence<RavenDBPersistence>();

        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Outbox>();

        busConfiguration.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();

        #endregion
    }

    void Setup3_5_0()
    {
#pragma warning disable 618

        #region PersistenceOrder_Explicit 5.0
        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.UsePersistence<NHibernatePersistence>()
            .For(Storage.Outbox);

        busConfiguration.UsePersistence<InMemoryPersistence>()
            .For(Storage.GatewayDeduplication);

        busConfiguration.UsePersistence<RavenDBPersistence>()
            .For(Storage.Sagas, Storage.Subscriptions, Storage.Timeouts);
        #endregion
#pragma warning restore 618

    }

    void Setup3_5_2()
    {
        #region PersistenceOrder_Explicit 5.2
        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Outbox>();

        busConfiguration.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();

        busConfiguration.UsePersistence<RavenDBPersistence, StorageType.Sagas>();
        busConfiguration.UsePersistence<RavenDBPersistence, StorageType.Subscriptions>();
        busConfiguration.UsePersistence<RavenDBPersistence, StorageType.Timeouts>();
        #endregion
    }

    void Setup2_5_0()
    {
#pragma warning disable 618

        #region PersistenceOrder_Incorrect 5.0
        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.UsePersistence<NHibernatePersistence>()
            .For(Storage.Outbox);

        busConfiguration.UsePersistence<InMemoryPersistence>()
            .For(Storage.GatewayDeduplication);

        // This one will override the above settings!
        busConfiguration.UsePersistence<RavenDBPersistence>();
        #endregion
#pragma warning restore 618
    }

    void Setup2_5_2()
    {
        #region PersistenceOrder_Incorrect 5.2
        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Outbox>();

        busConfiguration.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();
            
        // This one will override the above settings!
        busConfiguration.UsePersistence<RavenDBPersistence>();
        #endregion
    }
}
