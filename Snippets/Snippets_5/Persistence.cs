using NServiceBus;
using NServiceBus.Persistence;
using NHibernateNS = NServiceBus.NHibernate;

public class Persistence
{
    public void AllThePersistence()
    {

        #region ConfigurePersistenceV5

        // Configure to use InMemory for all persistence types
        Configure.With(b => b.UsePersistence<InMemory>());

        // Configure to use InMemory for specific persistence types
        Configure.With(b => b.UsePersistence<InMemory>()
            .For(Storage.Sagas, Storage.Subscriptions));

        // Configure to use NHibernate for all persistence types
        Configure.With(b => b.UsePersistence<NServiceBus.NHibernate>());

        // Configure to use NHibernate for specific persistence types
        Configure.With(b => b.UsePersistence<NServiceBus.NHibernate>()
            .For(Storage.Sagas, Storage.Subscriptions));

        // Configure to use RavenDB for all persistence types
        Configure.With(b => b.UsePersistence<RavenDB>());

        // Configure to use RavenDB for specific persistence types
        Configure.With(b => b.UsePersistence<RavenDB>()
            .For(Storage.Sagas, Storage.Subscriptions));

        #endregion
    }

}