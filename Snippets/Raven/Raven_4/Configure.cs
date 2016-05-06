using System;
using System.IO;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.RavenDB;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Document.DTC;

class Configure
{
    void SharedSessionForSagasAndOutbox(EndpointConfiguration endpointConfiguration)
    {
        #region ravendb-persistence-shared-session-for-sagas

        DocumentStore documentStore = new DocumentStore();
        // configure documentStore here
        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.UseSharedAsyncSession(() =>
        {
            IAsyncDocumentSession session = documentStore.OpenAsyncSession();
            // customize session here
            return session;
        });

        #endregion
    }

    public class MyMessage
    {
    }

    public class MyDocument
    {
    }

    #region ravendb-persistence-shared-session-for-sagas-handler

    public class MyMessageHandler : IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            MyDocument doc = new MyDocument();

            IAsyncDocumentSession ravenSession = context.SynchronizedStorageSession.RavenSession();
            return ravenSession.StoreAsync(doc);
        }
    }

    #endregion

    void SpecificExternalDocumentStore(EndpointConfiguration endpointConfiguration)
    {
        #region ravendb-persistence-specific-external-store

        DocumentStore documentStore = new DocumentStore();
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
        persistence.UseDocumentStoreForSubscriptions(readOnlySettings =>
        {
            DocumentStore documentStore = new DocumentStore();
            // configure documentStore here
            return documentStore;
        });
        persistence.UseDocumentStoreForSagas(readOnlySettings =>
        {
            DocumentStore documentStore = new DocumentStore();
            // configure documentStore here
            return documentStore;
        });
        persistence.UseDocumentStoreForTimeouts(readOnlySettings =>
        {
            DocumentStore documentStore = new DocumentStore();
            // configure documentStore here
            return documentStore;
        });
        persistence.UseDocumentStoreForGatewayDeduplication(readOnlySettings =>
        {
            DocumentStore documentStore = new DocumentStore();
            // configure documentStore here
            return documentStore;
        });

        #endregion
    }

    void SpecificDocumentStoreViaConnectionString()
    {
        //See the config file
    }

    void ExternalDocumentStore(EndpointConfiguration endpointConfiguration)
    {
        #region ravendb-persistence-external-store

        DocumentStore documentStore = new DocumentStore();
        // configure documentStore here
        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.SetDefaultDocumentStore(documentStore);

        #endregion
    }

    void ExternalConnectionParameters(EndpointConfiguration endpointConfiguration)
    {
        #region ravendb-persistence-external-connection-params

        ConnectionParameters connectionParams = new ConnectionParameters();
        // configure connection params (ApiKey, DatabaseName, Url) here
        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.SetDefaultDocumentStore(connectionParams);

        #endregion
    }

    void CreateDocumentStoreByFunc(EndpointConfiguration endpointConfiguration)
    {
        #region ravendb-persistence-create-store-by-func

        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.SetDefaultDocumentStore(readOnlySettings =>
        {
            DocumentStore documentStore = new DocumentStore();
            // configure documentStore here
            return documentStore;
        });

        #endregion
    }

    void SharedDocumentStoreViaConnectionString()
    {
        //See the config file
    }

    void ManualDtcSettingExample()
    {
        string UrlToRavenDB = "http://localhost:8080";

        #region RavenDBManualDtcSettingExample
        // Value must uniquely identify endpoint on the machine and remain stable on process restarts
        Guid resourceManagerId = new Guid("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");

        string dtcRecoveryBasePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        string recoveryPath = Path.Combine(dtcRecoveryBasePath, "NServiceBus.RavenDB", resourceManagerId.ToString());

        DocumentStore store = new DocumentStore
        {
            Url = UrlToRavenDB,
            ResourceManagerId = resourceManagerId,
            TransactionRecoveryStorage = new LocalDirectoryTransactionRecoveryStorage(recoveryPath)
        };
        store.Initialize();

        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("EndpointName");
        var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.SetDefaultDocumentStore(store);

        #endregion
    }
}