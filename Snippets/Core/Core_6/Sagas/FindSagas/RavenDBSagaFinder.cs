namespace Core6.Sagas.FindSagas
{
    using System.Linq;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Extensibility;
    using NServiceBus.Persistence;
    using NServiceBus.Sagas;

    public class RavenDBSagaFinder
    {
        #region ravendb-saga-finder

        public class MyRavenDbSagaFinder :
            IFindSagas<MySagaData>.Using<MyMessage>
        {
            public Task<MySagaData> FindBy(MyMessage message, SynchronizedStorageSession storageSession, ReadOnlyContextBag context)
            {
                // the custom finding logic here, e.g.
                var ravenSession = storageSession.RavenSession();
                var sagaData = ravenSession
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