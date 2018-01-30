using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.RavenDB;
using Raven.Client.Document;
using System;
using System.IO;
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

    void CustomizeDocumentSession(BusConfiguration busConfiguration)
    {
        #region ravendb-persistence-customize-document-session

        var myDocumentStore = new DocumentStore();
        // configure documentStore properties here

        var persistence = busConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.UseSharedSession(
            getSessionFunc: () =>
            {
                var session = myDocumentStore.OpenSession();
                // customize session here
                return session;
            });

        #endregion
    }

    void SpecificExternalDocumentStore(BusConfiguration busConfiguration)
    {
        #region ravendb-persistence-specific-external-store

        var documentStore = new DocumentStore();
        // configure documentStore here

        var persistence = busConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.UseDocumentStoreForSubscriptions(documentStore);
        persistence.UseDocumentStoreForSagas(documentStore);
        persistence.UseDocumentStoreForTimeouts(documentStore);

        #endregion
    }

    public void SpecificDocumentStoreViaConnectionString()
    {
        // See the config file
    }

    void ExternalDocumentStore(BusConfiguration busConfiguration)
    {
        #region ravendb-persistence-external-store

        var documentStore = new DocumentStore();
        // configure documentStore here

        var persistence = busConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.SetDefaultDocumentStore(documentStore);

        #endregion
    }

    void ExternalConnectionParameters(BusConfiguration busConfiguration)
    {
        #region ravendb-persistence-external-connection-params

        var connectionParams = new ConnectionParameters();
        // configure connection params (ApiKey, DatabaseName, Url) here

        var persistence = busConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.SetDefaultDocumentStore(connectionParams);

        #endregion
    }

    public void SharedDocumentStoreViaConnectionString()
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

        var busConfiguration = new BusConfiguration();
        var persistence = busConfiguration.UsePersistence<RavenDBPersistence>();
        persistence.SetDefaultDocumentStore(store);

        #endregion
    }


}