using NServiceBus;
using NServiceBus.Settings;
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
        //TODO: instances of DocumentStore should be disposed of at endpoint cleanup time

        #region ravendb-persistence-specific-create-store-by-func

        DocumentStore subscriptionStore;
        DocumentStore sagaStore;
        DocumentStore timeoutStore;
        DocumentStore gatewayStore;

        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.UseDocumentStoreForSubscriptions(
            storeCreator: (ReadOnlySettings readOnlySettings) =>
            {
                subscriptionStore = new DocumentStore();
                // configure documentStore here
                return subscriptionStore;
            });
        persistence.UseDocumentStoreForSagas(
            storeCreator: (ReadOnlySettings readOnlySettings) =>
            {
                sagaStore = new DocumentStore();
                // configure documentStore here
                return sagaStore;
            });
        persistence.UseDocumentStoreForTimeouts(
            storeCreator: (ReadOnlySettings readOnlySettings) =>
            {
                timeoutStore = new DocumentStore();
                // configure documentStore here
                return timeoutStore;
            });
        persistence.UseDocumentStoreForGatewayDeduplication(
            storeCreator: (ReadOnlySettings readOnlySettings) =>
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
            storeCreator: (ReadOnlySettings readOnlySettings) =>
            {
                documentStore = new DocumentStore();
                // configure documentStore here
                return documentStore;
            });

        #endregion
    }

    void ResolveSpecificDocumentStoreFromContainer(EndpointConfiguration endpointConfiguration)
    {
        //TODO: instances of DocumentStore should be disposed of at endpoint cleanup time

        #region ravendb-persistence-specific-resolve-from-container

        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        
        persistence.UseDocumentStoreForSubscriptions(
            storeCreator: builder => builder.Build<IDocumentStore>());
        persistence.UseDocumentStoreForSagas(
            storeCreator: builder => builder.Build<IDocumentStore>());
        persistence.UseDocumentStoreForTimeouts(
            storeCreator: builder => builder.Build<IDocumentStore>());
        persistence.UseDocumentStoreForGatewayDeduplication(
            storeCreator: builder => builder.Build<IDocumentStore>());

        #endregion
    }

    void ResolveDocumentStoreFromContainer(EndpointConfiguration endpointConfiguration)
    {
        #region ravendb-persistence-resolve-from-container

        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.SetDefaultDocumentStore(
            storeCreator: builder => builder.Build<IDocumentStore>());

        #endregion
    }

    void SharedDocumentStoreViaConnectionString()
    {
        // See the config file
    }
}