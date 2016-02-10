namespace Snippets6.Persistence.InMemory
{
    using NServiceBus;
    using NServiceBus.Persistence;

    class Usage
    {
        public void ConfiguringInMemory()
        {
            #region ConfiguringInMemory

            EndpointConfiguration configuration = new EndpointConfiguration();

            configuration.UsePersistence<InMemoryPersistence, StorageType.Sagas>();
            configuration.UsePersistence<InMemoryPersistence, StorageType.Subscriptions>();
            configuration.UsePersistence<InMemoryPersistence, StorageType.Timeouts>();
            configuration.UsePersistence<InMemoryPersistence, StorageType.Outbox>();
            configuration.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();

            #endregion
        }
    }
}