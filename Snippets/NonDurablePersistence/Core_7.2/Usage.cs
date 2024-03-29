﻿namespace Core7
{
    using NServiceBus;

    class Usage
    {
        public Usage(EndpointConfiguration endpointConfiguration)
        {
            #region GatewayDeduplicationCacheSize

            var persistence = endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();
            persistence.GatewayDeduplicationCacheSize(50000);

            #endregion
        }
    }
}
