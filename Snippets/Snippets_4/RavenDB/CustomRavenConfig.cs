using NServiceBus;
using NServiceBus.RavenDB;

namespace Snippets4.RavenDB
{

    class CustomRavenConfig
    {
        CustomRavenConfig(Configure configure)
        {
            #region OldRavenDBPersistenceInitialization

            configure.RavenPersistence();
            configure.RavenSagaPersister();
            configure.RavenSubscriptionStorage();
            configure.UseRavenTimeoutPersister();
            configure.UseRavenGatewayDeduplication();
            configure.UseRavenGatewayPersister();

            #endregion

            #region Version2_5RavenDBPersistenceInitialization

            // Need to call this method
            configure.RavenDBStorage();
            // Call this method to use Raven saga storage
            configure.UseRavenDBSagaStorage();
            // Call this method to use Raven subscription storage
            configure.UseRavenDBSubscriptionStorage();
            // Call this method to use Raven timeout storage
            configure.UseRavenDBTimeoutStorage();
            // Call this method to use Raven deduplication storage for the Gateway
            configure.UseRavenDBGatewayDeduplicationStorage();
            // Call this method to use the  Raven Gateway storage method
            configure.UseRavenDBGatewayStorage();

            #endregion
        }

    }
}