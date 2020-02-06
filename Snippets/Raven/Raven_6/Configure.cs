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
        persistence.UseDocumentStoreForGatewayDeduplication(documentStore);

        #endregion
    }

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
        persistence.UseDocumentStoreForGatewayDeduplication(
            readOnlySettings =>
            {
                gatewayStore = new DocumentStore();
                // configure documentStore here
                return gatewayStore;
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

    void ResolveSpecificDocumentStoreFromContainer(EndpointConfiguration endpointConfiguration)
    {
        #region ravendb-persistence-specific-create-store-by-func-6-1

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

    void ResolveDocumentStoreFromContainer(EndpointConfiguration endpointConfiguration)
    {
        #region ravendb-persistence-create-store-by-func-6-1

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

    void SharedDocumentStoreViaConnectionString()
    {
        // See the config file
    }
}