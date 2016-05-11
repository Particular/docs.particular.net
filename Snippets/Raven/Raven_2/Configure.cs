using System;
using System.IO;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.RavenDB;
using NServiceBus.RavenDB.Persistence;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Document.DTC;

class Configure
{
    void StaleSagas(BusConfiguration busConfiguration)
    {
        #region ravendb-persistence-stale-sagas

        var persistence = busConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.AllowStaleSagaReads();

        #endregion
    }

    void SharedSessionForSagasAndOutbox(BusConfiguration busConfiguration)
    {
        #region ravendb-persistence-shared-session-for-sagas

        DocumentStore documentStore = new DocumentStore();
        // configure documentStore properties here

        var persistence = busConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.UseSharedSession(() =>
        {
            IDocumentSession session = documentStore.OpenSession();
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
        ISessionProvider sessionProvider;

        public MyMessageHandler(ISessionProvider sessionProvider)
        {
            this.sessionProvider = sessionProvider;
        }

        public void Handle(MyMessage message)
        {
            MyDocument doc = new MyDocument();

            sessionProvider.Session.Store(doc);
        }
    }

    #endregion

    void SpecificExternalDocumentStore(BusConfiguration busConfiguration)
    {
        #region ravendb-persistence-specific-external-store

        DocumentStore documentStore = new DocumentStore();
        // configure documentStore here

        var persistence = busConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.UseDocumentStoreForSubscriptions(documentStore);
        persistence.UseDocumentStoreForSagas(documentStore);
        persistence.UseDocumentStoreForTimeouts(documentStore);

        #endregion
    }

    public void SpecificDocumentStoreViaConnectionString()
    {
        //See the config file
    }

    void ExternalDocumentStore(BusConfiguration busConfiguration)
    {
        #region ravendb-persistence-external-store

        DocumentStore documentStore = new DocumentStore();
        // configure documentStore here

        var persistence = busConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.SetDefaultDocumentStore(documentStore);

        #endregion
    }

    void ExternalConnectionParameters(BusConfiguration busConfiguration)
    {
        #region ravendb-persistence-external-connection-params

        ConnectionParameters connectionParams = new ConnectionParameters();
        // configure connection params (ApiKey, DatabaseName, Url) here

        var persistence = busConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.SetDefaultDocumentStore(connectionParams);

        #endregion
    }

    public void SharedDocumentStoreViaConnectionString()
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

        BusConfiguration busConfiguration = new BusConfiguration();
        var persistence = busConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.SetDefaultDocumentStore(store);

        #endregion
    }


}
