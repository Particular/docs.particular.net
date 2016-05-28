namespace Core6.Sagas.FindSagas
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Extensibility;
    using NServiceBus.Persistence;
    using NServiceBus.Sagas;

    public class NHibernateSagaFinder
    {
        #region nhibernate-saga-finder

        public class MyNHibernateSagaFinder :
            IFindSagas<MySagaData>.Using<MyMessage>
        {
            public Task<MySagaData> FindBy(MyMessage message, SynchronizedStorageSession storageSession, ReadOnlyContextBag context)
            {
                // the custom finding logic here, e.g.
                var nhibernateSession = storageSession.Session();
                var sagaData = nhibernateSession.QueryOver<MySagaData>()
                    .Where(x =>
                        x.SomeID == message.SomeID &&
                        x.SomeData == message.SomeData)
                    .SingleOrDefault();
                return Task.FromResult(sagaData);
            }
        }

        #endregion


    }
}