namespace Core6.Persistence.InMemory
{
    using NServiceBus;
    using NServiceBus.Persistence;

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
        }
    }
}