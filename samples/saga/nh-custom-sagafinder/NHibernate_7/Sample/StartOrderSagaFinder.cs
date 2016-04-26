using System.Threading.Tasks;
using NHibernate;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Persistence;
using NServiceBus.Sagas;

#region CustomSagaFinderNHibernate

class StartOrderSagaFinder : IFindSagas<OrderSagaData>.Using<StartOrder>
{

    public Task<OrderSagaData> FindBy(StartOrder message, SynchronizedStorageSession storageSession, ReadOnlyContextBag context)
    {
        ISession session = storageSession.Session();
        //if the instance is null a new saga will be automatically created if
        //the Saga handles the message as IAmStartedByMessages<StartOrder>; otherwise an exception is raised.
        OrderSagaData orderSagaData = session.QueryOver<OrderSagaData>()
            .Where(d => d.OrderId == message.OrderId)
            .SingleOrDefault();

        return Task.FromResult(orderSagaData);
    }
}

#endregion

