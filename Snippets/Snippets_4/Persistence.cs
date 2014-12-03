using NServiceBus;


public class Persistence
{
    public void AllThePersistence()
    {

        #region ConfigurePersistence

        // Configure to use InMemory 
        Configure.With().InMemorySagaPersister();
        Configure.With().UseInMemoryTimeoutPersister();
        Configure.With().InMemorySubscriptionStorage();
        Configure.With().RunGatewayWithInMemoryPersistence();
        Configure.With().UseInMemoryGatewayDeduplication();

        // Configure to use NHibernate
        Configure.With().UseNHibernateSagaPersister();
        Configure.With().UseNHibernateTimeoutPersister();
        Configure.With().UseNHibernateSubscriptionPersister();
        Configure.With().UseNHibernateGatewayPersister();
        Configure.With().UseNHibernateGatewayDeduplication();

        // Configure to use RavenDB for everything
        Configure.With().RavenPersistence();

        // Configure to use RavenDB
        Configure.With().RavenSagaPersister();
        Configure.With().UseRavenTimeoutPersister();
        Configure.With().RavenSubscriptionStorage();
        Configure.With().RunGatewayWithRavenPersistence();
        Configure.With().UseNHibernateGatewayDeduplication();

        #endregion
    }

}