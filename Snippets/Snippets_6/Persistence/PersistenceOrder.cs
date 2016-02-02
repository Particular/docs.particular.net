namespace Snippets6.Persistence
{
    using NServiceBus;
    using NServiceBus.Persistence;

    class PersistenceOrder
    {
        void Setup()
        {
            #region PersistenceOrder_Correct
            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration.UsePersistence<RavenDBPersistence>();

            busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Outbox>();

            busConfiguration.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();

            #endregion
        }

        void Setup3()
        {
            #region PersistenceOrder_Explicit
            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Outbox>();

            busConfiguration.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();

            busConfiguration.UsePersistence<RavenDBPersistence, StorageType.Sagas>();
            busConfiguration.UsePersistence<RavenDBPersistence, StorageType.Subscriptions>();
            busConfiguration.UsePersistence<RavenDBPersistence, StorageType.Timeouts>();
            #endregion
        }


        void Setup2()
        {
            #region PersistenceOrder_Incorrect
            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration.UsePersistence<NHibernatePersistence, StorageType.Outbox>();

            busConfiguration.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();
            
            // This one will override the above settings!
            busConfiguration.UsePersistence<RavenDBPersistence>();
            #endregion
        }
    }
}
