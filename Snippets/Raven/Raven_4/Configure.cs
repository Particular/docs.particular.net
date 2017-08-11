using System;
using System.IO;
using NServiceBus;
using NServiceBus.Persistence.RavenDB;
using Raven.Client.Document;
using Raven.Client.Document.DTC;

class Configure
{
    void SharedSessionForSagasAndOutbox(EndpointConfiguration endpointConfiguration)
    {
        #region ravendb-persistence-shared-session-for-sagas

        var documentStore = new DocumentStore();
        // configure documentStore here
        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.UseSharedAsyncSession(() =>
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

        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.UseDocumentStoreForSubscriptions(
            storeCreator: readOnlySettings =>
            {
                var documentStore = new DocumentStore();
                // configure documentStore here
                return documentStore;
            });
        persistence.UseDocumentStoreForSagas(
            storeCreator: readOnlySettings =>
            {
                var documentStore = new DocumentStore();
                // configure documentStore here
                return documentStore;
            });
        persistence.UseDocumentStoreForTimeouts(
            storeCreator: readOnlySettings =>
            {
                var documentStore = new DocumentStore();
                // configure documentStore here
                return documentStore;
            });
        persistence.UseDocumentStoreForGatewayDeduplication(
            storeCreator: readOnlySettings =>
            {
                var documentStore = new DocumentStore();
                // configure documentStore here
                return documentStore;
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

    void ExternalConnectionParameters(EndpointConfiguration endpointConfiguration)
    {
        #region ravendb-persistence-external-connection-params

        var connectionParams = new ConnectionParameters();
        // configure connection params (ApiKey, DatabaseName, Url) here
        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.SetDefaultDocumentStore(connectionParams);

        #endregion
    }

    void CreateDocumentStoreByFunc(EndpointConfiguration endpointConfiguration)
    {
        #region ravendb-persistence-create-store-by-func

        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.SetDefaultDocumentStore(
            storeCreator: readOnlySettings =>
            {
                var documentStore = new DocumentStore();
                // configure documentStore here
                return documentStore;
            });

        #endregion
    }

    void SharedDocumentStoreViaConnectionString()
    {
        // See the config file
    }

    void ManualDtcSettingExample()
    {
        var UrlToRavenDB = "http://localhost:8080";

        #region RavenDBManualDtcSettingExample

        // Value must uniquely identify endpoint on the machine and remain stable on process restarts
        var resourceManagerId = new Guid("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");

        var dtcRecoveryBasePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        var recoveryPath = Path.Combine(dtcRecoveryBasePath, "NServiceBus.RavenDB", resourceManagerId.ToString());

        var store = new DocumentStore
        {
            Url = UrlToRavenDB,
            ResourceManagerId = resourceManagerId,
            TransactionRecoveryStorage = new LocalDirectoryTransactionRecoveryStorage(recoveryPath)
        };
        store.Initialize();

        var endpointConfiguration = new EndpointConfiguration("EndpointName");
        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.SetDefaultDocumentStore(store);

        #endregion
    }
}