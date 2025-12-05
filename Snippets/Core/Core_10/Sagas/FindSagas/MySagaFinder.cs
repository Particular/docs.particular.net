using NServiceBus;

namespace Core.Sagas.FindSagas;

using System.Threading;
using System.Threading.Tasks;
using NServiceBus.Extensibility;
using NServiceBus.Persistence;
using NServiceBus.Sagas;

#region saga-finder

public class MySagaFinder :
    ISagaFinder<MySagaData, MyMessage>
{
    public Task<MySagaData> FindBy(MyMessage message, ISynchronizedStorageSession storageSession, IReadOnlyContextBag context, CancellationToken cancellationToken)
    {
        // SynchronizedStorageSession will have a persistence specific extension method
        // For example GetDbSession is a stub extension method
        var dbSession = storageSession.GetDbSession();
        return dbSession.GetSagaFromDB(message.SomeId, message.SomeData);
        // If a saga can't be found Task.FromResult(null) should be returned
    }
}

#endregion

class MyFinderSaga : Saga<MySagaData>
{
    #region saga-finder-mapping

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
    {
        mapper.ConfigureFinderMapping<MyMessage, MySagaFinder>();
    }

    #endregion
}