using System.Threading.Tasks;
using NServiceBus.Extensibility;
using NServiceBus.Persistence;
using NServiceBus.RavenDB.Persistence;
using NServiceBus.Sagas;
using Raven.Client;
using Raven.Client.UniqueConstraints;

#region CustomSagaFinderWithUniqueConstraintRavenDB

class StartOrderSagaFinder : IFindSagas<OrderSagaData>.Using<StartOrder>
{
    IAsyncSessionProvider sessionProvider;

    public StartOrderSagaFinder(IAsyncSessionProvider sessionProvider)
    {
        this.sessionProvider = sessionProvider;
    }

    public Task<OrderSagaData> FindBy(StartOrder message, SynchronizedStorageSession storageSession, ReadOnlyContextBag context)
    {
        IAsyncDocumentSession session = sessionProvider.AsyncSession;
        //if the instance is null a new saga will be automatically created if
        //the Saga handles the message as IAmStartedByMessages<StartOrder>; otherwise an exception is raised.
        return session.LoadByUniqueConstraintAsync<OrderSagaData>(d => d.OrderId, message.OrderId);
    }
}

#endregion
