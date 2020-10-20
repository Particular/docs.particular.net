namespace Core5
{
    using NServiceBus;
    using NServiceBus.Persistence;

    class Usage
    {
        void Version_5_0(BusConfiguration busConfiguration)
        {
#pragma warning disable 618

            #region ConfiguringNonDurable 5.0

            var persistence = busConfiguration.UsePersistence<InMemoryPersistence>();
            persistence.For(
                Storage.Sagas,
                Storage.Subscriptions,
                Storage.Timeouts,
                Storage.Outbox,
                Storage.GatewayDeduplication);

            #endregion

#pragma warning restore 618
        }

        void Version_5_2(BusConfiguration busConfiguration)
        {
            #region ConfiguringNonDurable 5.2

            busConfiguration.UsePersistence<InMemoryPersistence, StorageType.Sagas>();
            busConfiguration.UsePersistence<InMemoryPersistence, StorageType.Subscriptions>();
            busConfiguration.UsePersistence<InMemoryPersistence, StorageType.Timeouts>();
            busConfiguration.UsePersistence<InMemoryPersistence, StorageType.Outbox>();
            busConfiguration.UsePersistence<InMemoryPersistence, StorageType.GatewayDeduplication>();

            #endregion
        }
    }
}