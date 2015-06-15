using NServiceBus;
using NServiceBus.RavenDB;

namespace Snippets4.RavenDB
{

    public class CustomRavenConfig
    {
        public void Simple()
        {
            #region CustomRavenConfig
            Configure.With()
                .RavenPersistence("http://localhost:8080", "MyDatabase");
            #endregion

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

            #region Version2_5RavenDBPersistenceInitialization

            Configure.With()
                .DefaultBuilder()
                // Need to call this method
                .RavenDBStorage()
                // Call this method to use Raven saga storage
                .UseRavenDBSagaStorage()
                // Call this method to use Raven subscription storage
                .UseRavenDBSubscriptionStorage()
                // Call this method to use Raven timeout storage
                .UseRavenDBTimeoutStorage()
                // Call this method to use Raven deduplication storage for the Gateway
                .UseRavenDBGatewayDeduplicationStorage()
                // Call this method to use the  Raven Gateway storage method
                .UseRavenDBGatewayStorage();

            #endregion
        }

    }
}