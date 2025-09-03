namespace Core7
{
    using NServiceBus;

    class Usage
    {
        public Usage(EndpointConfiguration endpointConfiguration)
        {
            #region ConfiguringNonDurable

            endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.Sagas>();
            endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.Subscriptions>();
            endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.Timeouts>();
            endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.Outbox>();

            #endregion
        }

        #pragma warning disable CS0618 // deprecated API
        public void CacheSize(EndpointConfiguration endpointConfiguration)
        {
            #region GatewayDeduplicationCacheSize

            var persistence = endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();
            persistence.GatewayDeduplicationCacheSize(50000);

            #endregion
        }
        #pragma warning restore CS0618
    }
}
