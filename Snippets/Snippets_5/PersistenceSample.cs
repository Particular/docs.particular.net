using NServiceBus;
using NServiceBus.Persistence;
using NHibernateNS = NServiceBus.NHibernate;

public class PersistenceSample
{
    public void AllThePersistence()
    {
        // Configure to use InMemory for all persistence types
        Configure.With().UsePersistence<InMemory>();

        // Configure to use InMemory for specific persistence types
        Configure.With().UsePersistence<InMemory>(c => c.For(Storage.Sagas, Storage.Subscriptions));

        // Configure to use NHibernate for all persistence types
        Configure.With().UsePersistence<NServiceBus.NHibernate>();

        // Configure to use NHibernate for specific persistence types
        Configure.With().UsePersistence<NServiceBus.NHibernate>(c => c.For(Storage.Sagas, Storage.Subscriptions));

        // Configure to use InMemory for specific persistence types
        Configure.With().UsePersistence<InMemory>(c => c.For(Storage.Sagas, Storage.Subscriptions));

        // Configure to use InMemory for all persistence types
        Configure.With().UsePersistence<InMemory>();

        // Configure to use InMemory for specific persistence types
        //Configure.With().UsePersistence<>(c => c.For(Storage.Sagas, Storage.Subscriptions));


    }

}