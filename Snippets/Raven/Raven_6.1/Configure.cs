using NServiceBus;
using Raven.Client.Documents;

class Configure
{
    void CreateSpecificDocumentStoreByFunc(EndpointConfiguration endpointConfiguration)
    {
        #region ravendb-persistence-specific-create-store-by-func

        //TODO: instances of DocumentStore should be disposed of at endpoint cleanup time
        DocumentStore subscriptionStore;
        DocumentStore sagaStore;
        DocumentStore timeoutStore;
        DocumentStore gatewayStore;

        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.UseDocumentStoreForSubscriptions(
            (settings, builder) =>
            {
                subscriptionStore = new DocumentStore();
                // configure documentStore here
                return subscriptionStore;
            });
        persistence.UseDocumentStoreForSagas(
            (settings, builder) =>
            {
                sagaStore = new DocumentStore();
                // configure documentStore here
                return sagaStore;
            });
        persistence.UseDocumentStoreForTimeouts(
            (settings, builder) =>
            {
                timeoutStore = new DocumentStore();
                // configure documentStore here
                return timeoutStore;
            });
        persistence.UseDocumentStoreForGatewayDeduplication(
            (settings, builder) =>
            {
                gatewayStore = new DocumentStore();
                // configure documentStore here
                return gatewayStore;
            });

        #endregion
    }

    void CreateDocumentStoreByFunc(EndpointConfiguration endpointConfiguration)
    {
        #region ravendb-persistence-create-store-by-func

        DocumentStore documentStore;
        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.SetDefaultDocumentStore(
            (settings, builder) =>
            {
                documentStore = new DocumentStore();
                // configure documentStore here
                return documentStore;
            });

        #endregion
    }
}