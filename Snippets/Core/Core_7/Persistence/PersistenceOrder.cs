﻿namespace Core6.Persistence
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

            endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();

            #endregion
        }

        void Setup3(EndpointConfiguration endpointConfiguration)
        {
            #region PersistenceOrder_Explicit

            endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.Outbox>();

            endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();

            endpointConfiguration.UsePersistence<RavenDBPersistence, StorageType.Sagas>();
            endpointConfiguration.UsePersistence<RavenDBPersistence, StorageType.Subscriptions>();
            endpointConfiguration.UsePersistence<RavenDBPersistence, StorageType.Timeouts>();
            #endregion
        }


        void Setup2(EndpointConfiguration endpointConfiguration)
        {
            #region PersistenceOrder_Incorrect

            endpointConfiguration.UsePersistence<NHibernatePersistence, StorageType.Outbox>();

            endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();

            // This one will override the above settings
            endpointConfiguration.UsePersistence<RavenDBPersistence>();
            #endregion
        }
    }
}
