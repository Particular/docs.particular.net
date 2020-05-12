namespace Core7.Persistence
{
    using NServiceBus;
    using NServiceBus.Persistence;

    class PersistenceOrder
    {
        void Setup(EndpointConfiguration endpointConfiguration)
        {
            #region PersistenceOrder_Correct

            endpointConfiguration.UsePersistence<RavenDBPersistence>();

            endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.Outbox>();

            endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.Subscriptions>();

            #endregion
        }

        void Setup3(EndpointConfiguration endpointConfiguration)
        {
            #region PersistenceOrder_Explicit

            endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.Outbox>();

            endpointConfiguration.UsePersistence<RavenDBPersistence, StorageType.Sagas>();
            endpointConfiguration.UsePersistence<RavenDBPersistence, StorageType.Subscriptions>();
            endpointConfiguration.UsePersistence<RavenDBPersistence, StorageType.Timeouts>();
            #endregion
        }


        void Setup2(EndpointConfiguration endpointConfiguration)
        {
            #region PersistenceOrder_Incorrect

            endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.Outbox>();

            endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.Subscriptions>();

            // This one will override the above settings
            endpointConfiguration.UsePersistence<RavenDBPersistence>();
            #endregion
        }
    }

    class RavenDBPersistence : PersistenceDefinition
    {
    }

    class NHibernatePersistence:PersistenceDefinition
    {
    }
}