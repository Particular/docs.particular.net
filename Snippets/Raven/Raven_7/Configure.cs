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
        DocumentStore timeoutStore;

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
        persistence.UseDocumentStoreForTimeouts(
            readOnlySettings =>
            {
                timeoutStore = new DocumentStore();
                // configure documentStore here
                return timeoutStore;
            });

        #endregion
    }

    void SpecificDocumentStoreViaConnectionString()
    {
        // See the config file
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

    void SharedDocumentStoreViaConnectionString()
    {
        // See the config file
    }
}