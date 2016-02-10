namespace Snippets6.Persistence
{
    using NServiceBus;
    using NServiceBus.Persistence;

    class PersistenceOrder
    {
        void Setup()
        {
            #region PersistenceOrder_Correct
            EndpointConfiguration configuration = new EndpointConfiguration();

            configuration.UsePersistence<RavenDBPersistence>();

            configuration.UsePersistence<NHibernatePersistence, StorageType.Outbox>();

            configuration.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();

            #endregion
        }

        void Setup3()
        {
            #region PersistenceOrder_Explicit
            EndpointConfiguration configuration = new EndpointConfiguration();

            configuration.UsePersistence<NHibernatePersistence, StorageType.Outbox>();

            configuration.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();

            configuration.UsePersistence<RavenDBPersistence, StorageType.Sagas>();
            configuration.UsePersistence<RavenDBPersistence, StorageType.Subscriptions>();
            configuration.UsePersistence<RavenDBPersistence, StorageType.Timeouts>();
            #endregion
        }


        void Setup2()
        {
            #region PersistenceOrder_Incorrect
            EndpointConfiguration configuration = new EndpointConfiguration();

            configuration.UsePersistence<NHibernatePersistence, StorageType.Outbox>();

            configuration.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();
            
            // This one will override the above settings!
            configuration.UsePersistence<RavenDBPersistence>();
            #endregion
        }
    }
}
