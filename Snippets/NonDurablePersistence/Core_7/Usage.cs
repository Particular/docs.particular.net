﻿namespace Core7
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
    }
}
