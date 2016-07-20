namespace Core5.Sagas.FindSagas
{
    using System.Linq;
    using NServiceBus.RavenDB.Persistence;
    using NServiceBus.Saga;

    public class RavenDBSagaFinder
    {
        #region ravendb-saga-finder

        public class MyRavenDbSagaFinder :
            IFindSagas<MySagaData>.Using<MyMessage>
        {
            ISessionProvider sessionProvider;

            public MyRavenDbSagaFinder(ISessionProvider sessionProvider)
            {
                this.sessionProvider = sessionProvider;
            }

            public MySagaData FindBy(MyMessage message)
            {
                // the custom finding logic here, e.g.
                return sessionProvider.Session
                    .Query<MySagaData>()
                    .SingleOrDefault(x =>
                        x.SomeID == message.SomeID &&
                        x.SomeData == message.SomeData);
            }
        }

        #endregion


    }
}