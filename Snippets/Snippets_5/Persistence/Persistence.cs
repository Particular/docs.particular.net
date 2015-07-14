namespace Snippets5.Persistence
{
    using NServiceBus;
    using NServiceBus.Persistence;

    public class Persistence
    {
        public void AllThePersistence()
        {
#pragma warning disable 618

            #region ConfigurePersistence

            BusConfiguration busConfiguration = new BusConfiguration();

            // Configure to use InMemory for all persistence types
            busConfiguration.UsePersistence<InMemoryPersistence>();

            // Configure to use InMemory for specific persistence types
            busConfiguration.UsePersistence<InMemoryPersistence>()
                .For(Storage.Sagas, Storage.Subscriptions);

            // Configure to use NHibernate for all persistence types
            busConfiguration.UsePersistence<NHibernatePersistence>();

            // Configure to use NHibernate for specific persistence types
            busConfiguration.UsePersistence<NHibernatePersistence>()
                .For(Storage.Sagas, Storage.Subscriptions);

            // Configure to use RavenDB for all persistence types
            busConfiguration.UsePersistence<RavenDBPersistence>();

            // Configure to use RavenDB for specific persistence types
            busConfiguration.UsePersistence<RavenDBPersistence>()
                .For(Storage.Sagas, Storage.Subscriptions);

            #endregion
#pragma warning restore 618
        }

    }
}