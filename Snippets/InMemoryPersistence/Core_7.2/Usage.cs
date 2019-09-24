namespace Core7
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region GatewayDeduplicationCacheSize

            var persistence = endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();
            persistence.GatewayDeduplicationCacheSize(50000); // Present since 7.1.10

            #endregion
        }
    }
}
