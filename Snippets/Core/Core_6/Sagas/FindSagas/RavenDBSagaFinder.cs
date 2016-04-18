namespace Core6.Sagas.FindSagas
{
    using System.Linq;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Extensibility;
    using NServiceBus.Persistence;
    using NServiceBus.Sagas;
    using Raven.Client;

    public class RavenDBSagaFinder
    {
        #region ravendb-saga-finder

        public class MyRavenDbSagaFinder :
            IFindSagas<MySagaData>.Using<MyMessage>
        {
            public Task<MySagaData> FindBy(MyMessage message, SynchronizedStorageSession storageSession, ReadOnlyContextBag context)
            {
                //your custom finding logic here, e.g.
                IAsyncDocumentSession ravenSession = storageSession.RavenSession();
                MySagaData sagaData = ravenSession
                    .Query<MySagaData>()
                    .SingleOrDefault(x =>
                        x.SomeID == message.SomeID &&
                        x.SomeData == message.SomeData);
                return Task.FromResult(sagaData);
            }
        }

        #endregion


    }
}