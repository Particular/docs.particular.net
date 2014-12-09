using NServiceBus;
using NServiceBus.Persistence;

public class Persistence
{
    public void AllThePersistence()
    {

        #region ConfigurePersistence

        var configuration = new BusConfiguration();

        // Configure to use InMemory for all persistence types
        configuration.UsePersistence<InMemoryPersistence>();

        // Configure to use InMemory for specific persistence types
        configuration.UsePersistence<InMemoryPersistence>()
            .For(Storage.Sagas, Storage.Subscriptions);

        // Configure to use NHibernate for all persistence types
        configuration.UsePersistence<NHibernatePersistence>();

        // Configure to use NHibernate for specific persistence types
        configuration.UsePersistence<NHibernatePersistence>()
            .For(Storage.Sagas, Storage.Subscriptions);

        // Configure to use RavenDB for all persistence types
        configuration.UsePersistence<RavenDBPersistence>();

        // Configure to use RavenDB for specific persistence types
        configuration.UsePersistence<RavenDBPersistence>()
            .For(Storage.Sagas, Storage.Subscriptions);

        #endregion
    }

}