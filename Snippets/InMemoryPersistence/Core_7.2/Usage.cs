namespace Core7
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region ConfiguringInMemory

            endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.Sagas>();
            endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.Subscriptions>();
            endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.Timeouts>();
            endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.Outbox>();
            endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();

            #endregion

            #region GatewayDeduplicationCacheSize

            var persistence = endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();
            persistence.GatewayDeduplicationCacheSize(50000); // Present since 7.1.10

            #endregion
        }
    }
}