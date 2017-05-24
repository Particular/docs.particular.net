﻿namespace Core6.Sagas.FindSagas
{
    using System.Threading.Tasks;
    using NServiceBus.Extensibility;
    using NServiceBus.Persistence;
    using NServiceBus.Sagas;

    #region saga-finder

    public class MySagaFinder :
        IFindSagas<MySagaData>.Using<MyMessage>
    {
        public Task<MySagaData> FindBy(MyMessage message, SynchronizedStorageSession storageSession, ReadOnlyContextBag context)
        {
            // SynchronizedStorageSession will have a persistence specific extension method
            // For example purposes GetDbSession is a stub extension method
            var dbSession = storageSession.GetDbSession();
            return dbSession.GetSagaFromDB(message.SomeId, message.SomeData);
            // If a saga can't be found Task.FromResult(null) should be returned
        }
    }

    #endregion
}