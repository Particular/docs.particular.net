using System.Threading.Tasks;
using NServiceBus.Extensibility;
using NServiceBus.Persistence;
using NServiceBus.RavenDB.Persistence;
using NServiceBus.Sagas;
using Raven.Client;
using Raven.Client.UniqueConstraints;

class CompleteOrderSagaFinder : IFindSagas<OrderSagaData>.Using<CompleteOrder>
{
    IAsyncSessionProvider sessionProvider;

    public CompleteOrderSagaFinder(IAsyncSessionProvider sessionProvider)
    {
        this.sessionProvider = sessionProvider;
    }

    public Task<OrderSagaData> FindBy(CompleteOrder message, SynchronizedStorageSession storageSession, ReadOnlyContextBag context)
    {
        IAsyncDocumentSession session = sessionProvider.AsyncSession;
        return session.LoadByUniqueConstraintAsync<OrderSagaData>(d => d.OrderId, message.OrderId);
    }
}
