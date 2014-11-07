using NServiceBus;
using NServiceBus.RavenDB;

public class UseRavenDBVersion2PersistenceUsingCustomInitialization : IWantCustomInitialization
{
    public void Init()
    {

        #region OldRavenDBPersistenceInitialization

        Configure.With()
           .DefaultBuilder()
           .RavenPersistence()
           .RavenSagaPersister()
           .RavenSubscriptionStorage()
           .UseRavenTimeoutPersister()
           .UseRavenGatewayDeduplication()
           .UseRavenGatewayPersister();

        #endregion 
    }
}

public class UseRavenDBVersion2_5PersistenceUsingCustomInitialization : IWantCustomInitialization
{
    public void Init()
    {
        #region Version2_5RavenDBPersistenceInitialization

        Configure.With()
           .DefaultBuilder()
           .RavenDBStorage() // Need to call this method
           .UseRavenDBSagaStorage() // Call this method to use Raven saga storage
           .UseRavenDBSubscriptionStorage() // Call this method to use Raven subscription storage
           .UseRavenDBTimeoutStorage() // Call this method to use Raven timeout storage
           .UseRavenDBGatewayDeduplicationStorage() // Call this method to use Raven deduplication storage for the Gateway
           .UseRavenDBGatewayStorage(); // Call this method to use the  Raven Gateway storage method
        #endregion
    }
}