using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Persistence;
using NServiceBus.Sagas;

class CompleteOrderSagaFinder : IFindSagas<OrderSagaData>.Using<CompleteOrder>
{
    public Task<OrderSagaData> FindBy(CompleteOrder message, SynchronizedStorageSession storageSession, ReadOnlyContextBag context)
    {
        var orderSagaData = storageSession.Session().QueryOver<OrderSagaData>()
            .Where(d => d.OrderId == message.OrderId)
            .SingleOrDefault();
        return Task.FromResult(orderSagaData);
    }
}