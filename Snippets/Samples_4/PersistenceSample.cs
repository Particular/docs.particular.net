using NServiceBus;

public class PersistenceSample
{
    public void AllThePersistence()
    {

        // Configure to use InMemory 
        Configure.With().InMemorySagaPersister();
        Configure.With().UseInMemoryTimeoutPersister();
        Configure.With().InMemorySubscriptionStorage();
        Configure.With().RunGatewayWithInMemoryPersistence();
        Configure.With().UseInMemoryGatewayDeduplication();

        // Configure to use NHibernate
        Configure.With().RavenSagaPersister();
        Configure.With().UseRavenTimeoutPersister();
        Configure.With().RavenSubscriptionStorage();
        Configure.With().RunGatewayWithRavenPersistence();
        Configure.With().UseNHibernateGatewayDeduplication();

        // Configure to use NHibernate
        Configure.With().UseNHibernateSagaPersister();
        Configure.With().UseNHibernateTimeoutPersister();
        Configure.With().UseNHibernateSubscriptionPersister();
        Configure.With().UseNHibernateGatewayPersister();
        Configure.With().UseNHibernateGatewayDeduplication();

    }

}