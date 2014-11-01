namespace Snippets_5.Persistence.RavenDB
{
    using NServiceBus;
    using NServiceBus.Persistence;
    using NServiceBus.RavenDB;
    using NServiceBus.RavenDB.Persistence;
    using Raven.Client.Document;

    class Configure
    {
        public void StaleSagas()
        {
            //Allows to correlate sagas on non-unique properties

            #region ravendb-persistence-stale-sagas

            var config = new BusConfiguration();
            config.UsePersistence<RavenDBPersistence>()
                .AllowStaleSagaReads();

            #endregion
        }

        public void SharedSessionForSagasAndOutbox()
        {
            #region ravendb-persistence-shared-session-for-sagas

            var myDocumentStore = new DocumentStore();
            // configure document store properties here

            var config = new BusConfiguration();
            config.UsePersistence<RavenDBPersistence>().UseSharedSession(() =>
                {
                    var session = myDocumentStore.OpenSession();
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
                var doc = new MyDocument();

                SessionProvider.Session.Store(doc);
            }
        }
        #endregion

        public void SpecificExternalDocumentStore()
        {
            #region ravendb-persistence-specific-external-store

            var myDocumentStore = new DocumentStore();
            // configure document store properties here

            var config = new BusConfiguration();
            config.UsePersistence<RavenDBPersistence>()
                  .UseDocumentStoreForSubscriptions(myDocumentStore)
                  .UseDocumentStoreForSagas(myDocumentStore)
                  .UseDocumentStoreForTimeouts(myDocumentStore);

            #endregion
        }

        public void SpeicifcDocumentStoreViaConnectionString()
        {
            //See the config file
        }

        public void ExternalDocumentStore()
        {
            #region ravendb-persistence-external-store

            var myDocumentStore = new DocumentStore();
            // configure document store properties here


            var config = new BusConfiguration();
            config.UsePersistence<RavenDBPersistence>()
                .SetDefaultDocumentStore(myDocumentStore);

            #endregion
        }

        public void ExternalConnectionParameters()
        {
            #region ravendb-persistence-external-connection-params

            var connectionParams = new ConnectionParameters();
            // configure connection params (ApiKey, DatabaseName, Url) here

            var config = new BusConfiguration();
            config.UsePersistence<RavenDBPersistence>()
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

            var config = new BusConfiguration();
            config.UsePersistence<RavenDBPersistence>();

            #endregion
        }
    }
}
