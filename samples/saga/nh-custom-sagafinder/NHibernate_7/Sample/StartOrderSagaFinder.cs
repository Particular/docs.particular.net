using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Persistence;
using NServiceBus.Sagas;

#region CustomSagaFinderNHibernate

class StartOrderSagaFinder :
    IFindSagas<OrderSagaData>.Using<StartOrder>
{

    public Task<OrderSagaData> FindBy(StartOrder message, SynchronizedStorageSession storageSession, ReadOnlyContextBag context)
    {
        var session = storageSession.Session();
        //if the instance is null a new saga will be automatically created if
        //the Saga handles the message as IAmStartedByMessages<StartOrder>; otherwise an exception is raised.
        var orderSagaData = session.QueryOver<OrderSagaData>()
            .Where(d => d.OrderId == message.OrderId)
            .SingleOrDefault();

        return Task.FromResult(orderSagaData);
    }
}

#endregion

