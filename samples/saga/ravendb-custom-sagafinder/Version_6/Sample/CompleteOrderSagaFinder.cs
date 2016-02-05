using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Persistence;
using NServiceBus.Sagas;
using Raven.Client;
using Raven.Client.UniqueConstraints;

class CompleteOrderSagaFinder : IFindSagas<OrderSagaData>.Using<CompleteOrder>
{
    public Task<OrderSagaData> FindBy(CompleteOrder message, SynchronizedStorageSession storageSession, ReadOnlyContextBag context)
    {
        IAsyncDocumentSession session = storageSession.Session();
        return session.LoadByUniqueConstraintAsync<OrderSagaData>(d => d.OrderId, message.OrderId);
    }
}
