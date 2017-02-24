using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Persistence;
using NServiceBus.Sagas;
using Raven.Client.UniqueConstraints;

class CompleteOrderSagaFinder :
    IFindSagas<OrderSagaData>.Using<CompleteOrder>
{
    public Task<OrderSagaData> FindBy(CompleteOrder message, SynchronizedStorageSession storageSession, ReadOnlyContextBag context)
    {
        var session = storageSession.RavenSession();
        return session.LoadByUniqueConstraintAsync<OrderSagaData>(d => d.OrderId, message.OrderId);
    }
}