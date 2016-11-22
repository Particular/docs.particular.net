namespace Core5.Persistence.Msmq
{
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.Persistence;
    using NServiceBus.Persistence.Legacy;

    class Usage
    {
        void Configuration(BusConfiguration busConfiguration)
        {
            #region ConfiguringMsmqPersistence

            busConfiguration.UsePersistence<MsmqPersistence>();

            #endregion
        }

        void WithDisabledTimeoutManager(BusConfiguration busConfiguration)
        {
            #region DisablingTimeoutManagerForMsmqPersistence

            busConfiguration.DisableFeature<TimeoutManager>();
            busConfiguration.UsePersistence<MsmqPersistence>();

            #endregion
        }

        void WithOtherPersisters(BusConfiguration busConfiguration)
        {
            #region MsmqPersistenceWithOtherPersisters

            busConfiguration.UsePersistence<InMemoryPersistence, StorageType.Sagas>();
            busConfiguration.UsePersistence<InMemoryPersistence, StorageType.Timeouts>();
            busConfiguration.UsePersistence<MsmqPersistence, StorageType.Subscriptions>();

            #endregion
        }
    }
}