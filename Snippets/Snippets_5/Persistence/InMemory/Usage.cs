namespace Snippets5.Persistence.InMemory
{
    using NServiceBus;
    using NServiceBus.Persistence;

    class Usage
    {
        public void Version_5_0()
        {
#pragma warning disable 618

            #region ConfiguringInMemory 5.0

            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration.UsePersistence<InMemoryPersistence>()
                .For(
                    Storage.Sagas,
                    Storage.Subscriptions,
                    Storage.Timeouts,
                    Storage.Outbox,
                    Storage.GatewayDeduplication);

            #endregion

#pragma warning restore 618

        }

        public void Version_5_2()
        {
            #region ConfiguringInMemory 5.2

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