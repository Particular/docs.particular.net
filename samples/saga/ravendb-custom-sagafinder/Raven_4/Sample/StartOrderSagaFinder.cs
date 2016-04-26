using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Persistence;
using NServiceBus.Sagas;
using Raven.Client;
using Raven.Client.UniqueConstraints;

#region CustomSagaFinderWithUniqueConstraintRavenDB

class StartOrderSagaFinder : IFindSagas<OrderSagaData>.Using<StartOrder>
{
    public Task<OrderSagaData> FindBy(StartOrder message, SynchronizedStorageSession storageSession, ReadOnlyContextBag context)
    {
        IAsyncDocumentSession session = storageSession.RavenSession();
        //if the instance is null a new saga will be automatically created if
        //the Saga handles the message as IAmStartedByMessages<StartOrder>; otherwise an exception is raised.
        return session.LoadByUniqueConstraintAsync<OrderSagaData>(d => d.OrderId, message.OrderId);
    }
}

#endregion
