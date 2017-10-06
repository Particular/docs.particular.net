namespace Core6.Persistence.Msmq
{
    using NServiceBus;
    using NServiceBus.Features;

    class Usage
    {
        void Configuration(EndpointConfiguration endpointConfiguration)
        {
            #region ConfiguringMsmqPersistence

            endpointConfiguration.UsePersistence<MsmqPersistence>();

            #endregion
        }

        void WithDisabledTimeoutManager(EndpointConfiguration endpointConfiguration)
        {
            #region DisablingTimeoutManagerForMsmqPersistence

            endpointConfiguration.DisableFeature<TimeoutManager>();
            endpointConfiguration.UsePersistence<MsmqPersistence>();

            #endregion
        }

        void WithOtherPersisters(EndpointConfiguration endpointConfiguration)
        {
            #region MsmqPersistenceWithOtherPersisters

            endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.Sagas>();
            endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.Timeouts>();
            endpointConfiguration.UsePersistence<MsmqPersistence, StorageType.Subscriptions>();

            #endregion
        }

        void OverrideSubscriptionQueue(EndpointConfiguration endpointConfiguration)
        {
            #region MsmqSubscriptionCode

            var persistence = endpointConfiguration.UsePersistence<MsmqPersistence>();
            persistence.SubscriptionQueue("YourEndpointName.Subscriptions");

            #endregion
        }
    }
}