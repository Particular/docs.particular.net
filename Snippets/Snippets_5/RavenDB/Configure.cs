namespace Snippets5.RavenDB
{
    using NServiceBus;
    using NServiceBus.Persistence;
    using NServiceBus.RavenDB;
    using NServiceBus.RavenDB.Persistence;
    using Raven.Client;
    using Raven.Client.Document;

    class RavenDBConfigure
    {
        public void StaleSagas()
        {
            //Allows to correlate sagas on non-unique properties

            #region ravendb-persistence-stale-sagas

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UsePersistence<RavenDBPersistence>()
                .AllowStaleSagaReads();

            #endregion
        }

        public void SharedSessionForSagasAndOutbox()
        {
            #region ravendb-persistence-shared-session-for-sagas

            DocumentStore myDocumentStore = new DocumentStore();
            // configure document store properties here

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UsePersistence<RavenDBPersistence>().UseSharedSession(() =>
            {
                IDocumentSession session = myDocumentStore.OpenSession();
                // customize the session properties here
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
            public ISessionProvider SessionProvider { get; set; }

            public void Handle(MyMessage message)
            {
                MyDocument doc = new MyDocument();

                SessionProvider.Session.Store(doc);
            }
        }

        #endregion

        public void SpecificExternalDocumentStore()
        {
            #region ravendb-persistence-specific-external-store

            DocumentStore myDocumentStore = new DocumentStore();
            // configure document store properties here

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UsePersistence<RavenDBPersistence>()
                .UseDocumentStoreForSubscriptions(myDocumentStore)
                .UseDocumentStoreForSagas(myDocumentStore)
                .UseDocumentStoreForTimeouts(myDocumentStore);

            #endregion
        }

        public void SpecificDocumentStoreViaConnectionString()
        {
            //See the config file
        }

        public void ExternalDocumentStore()
        {
            #region ravendb-persistence-external-store

            DocumentStore myDocumentStore = new DocumentStore();
            // configure document store properties here


            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UsePersistence<RavenDBPersistence>()
                .SetDefaultDocumentStore(myDocumentStore);

            #endregion
        }

        public void ExternalConnectionParameters()
        {
            #region ravendb-persistence-external-connection-params

            ConnectionParameters connectionParams = new ConnectionParameters();
            // configure connection params (ApiKey, DatabaseName, Url) here

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UsePersistence<RavenDBPersistence>()
                .SetDefaultDocumentStore(connectionParams);

            #endregion
        }

        public void SharedDocumentStoreViaConnectionString()
        {
            //See the config file
        }

        public void Default()
        {
            //Connects to http://localhost:8080 by default

            #region ravendb-persistence-default

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UsePersistence<RavenDBPersistence>();

            #endregion
        }
    }
}