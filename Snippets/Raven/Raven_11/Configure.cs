using NServiceBus;
using Raven.Client.Documents;

class Configure
{
    void CustomizeDocumentSession(EndpointConfiguration endpointConfiguration)
    {
        #region ravendb-persistence-customize-document-session

        var documentStore = new DocumentStore();
        // configure documentStore here
        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.UseSharedAsyncSession(
            getAsyncSessionFunc: headers =>
            {
                var session = documentStore.OpenAsyncSession();
                // customize session here
                return session;
            });

        #endregion
    }

    void SpecificExternalDocumentStore(EndpointConfiguration endpointConfiguration)
    {
        #region ravendb-persistence-specific-external-store

        var documentStore = new DocumentStore();
        // configure documentStore here
        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.UseDocumentStoreForSubscriptions(documentStore);
        persistence.UseDocumentStoreForSagas(documentStore);

        #endregion
    }

    void CreateSpecificDocumentStoreByFunc(EndpointConfiguration endpointConfiguration)
    {
        #region ravendb-persistence-specific-create-store-by-func

        //TODO: instances of DocumentStore should be disposed of at endpoint cleanup time
        DocumentStore subscriptionStore;
        DocumentStore sagaStore;

        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.UseDocumentStoreForSubscriptions(
            readOnlySettings =>
            {
                subscriptionStore = new DocumentStore();
                // configure documentStore here
                return subscriptionStore;
            });
        persistence.UseDocumentStoreForSagas(
            readOnlySettings =>
            {
                sagaStore = new DocumentStore();
                // configure documentStore here
                return sagaStore;
            });

        #endregion
    }

    void ExternalDocumentStore(EndpointConfiguration endpointConfiguration)
    {
        #region ravendb-persistence-external-store

        var documentStore = new DocumentStore();
        // configure documentStore here
        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.SetDefaultDocumentStore(documentStore);

        #endregion
    }

    void CreateDocumentStoreByFunc(EndpointConfiguration endpointConfiguration)
    {
        #region ravendb-persistence-create-store-by-func

        DocumentStore documentStore;
        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.SetDefaultDocumentStore(
            readOnlySettings =>
            {
                documentStore = new DocumentStore();
                // configure documentStore here
                return documentStore;
            });

        #endregion
    }

    void OptimisticConcurrency(EndpointConfiguration endpointConfiguration)
    {
        #region ravendb-persistence-optimistic-concurrency

        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        var sagas = persistence.Sagas();
        sagas.UseOptimisticLocking();

        #endregion
    }

    void EnableClusterWideTransactions(EndpointConfiguration endpointConfiguration)
    {
        #region ravendb-persistence-cluster-wide-transactions

        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.EnableClusterWideTransactions();

        #endregion
    }
}