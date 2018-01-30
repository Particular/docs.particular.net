namespace Core3.Sagas.FindSagas
{
    using NServiceBus.Saga;

    #region saga-finder

    public class MySagaFinder :
        IFindSagas<MySagaData>.Using<MyMessage>
    {
        DbSessionProvider sessionProvider;

        // Inject the persistence specific session provider
        // For example purposes DbSessionProvider is a stub
        public MySagaFinder(DbSessionProvider sessionProvider)
        {
            this.sessionProvider = sessionProvider;
        }

        public MySagaData FindBy(MyMessage message)
        {
            // Use the injected sessionProvider to get a db session
            // For example GetDbSession is a stub extension method
            var dbSession = sessionProvider.GetDbSession();
            return dbSession.GetSagaFromDB(message.SomeId, message.SomeData);
            // If a saga can't be found null should be returned
        }
    }

    #endregion
}
