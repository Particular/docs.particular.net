namespace Snippets6.Persistence.InMemory
{
    using NServiceBus;
    using NServiceBus.Persistence;

    class Usage
    {
        public void ConfiguringInMemory()
        {
            #region ConfiguringInMemory

            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration.UsePersistence<InMemoryPersistence, StorageType.Sagas>();
            busConfiguration.UsePersistence<InMemoryPersistence, StorageType.Subscriptions>();
            busConfiguration.UsePersistence<InMemoryPersistence, StorageType.Timeouts>();
            busConfiguration.UsePersistence<InMemoryPersistence, StorageType.Outbox>();
            busConfiguration.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();

            #endregion
        }
    }
}