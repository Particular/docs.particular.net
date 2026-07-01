namespace NonDurablePersistence_3
{
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Extensibility;
    using NServiceBus.Persistence;
    using NServiceBus.Persistence.NonDurable;
    using NServiceBus.Sagas;

    #region NonDurableSagaProjection

    class OrderSagaFinder :
        ISagaFinder<OrderSagaData, CompleteOrder>
    {
        public Task<OrderSagaData> FindBy(CompleteOrder message, ISynchronizedStorageSession session, IReadOnlyContextBag context, CancellationToken cancellationToken = default)
        {
            var nonDurableSession = session.NonDurablePersistenceSession();

            var sagaData = nonDurableSession.GetSagaData<OrderSagaData>(
                context,
                data => data.OrderId == message.OrderId,
                cancellationToken);

            return Task.FromResult(sagaData);
        }
    }

    class OrderSagaFinderWithState :
        ISagaFinder<OrderSagaData, CompleteOrder>
    {
        public Task<OrderSagaData> FindBy(CompleteOrder message, ISynchronizedStorageSession session, IReadOnlyContextBag context, CancellationToken cancellationToken = default)
        {
            var nonDurableSession = session.NonDurablePersistenceSession();

            var sagaData = nonDurableSession.GetSagaData<OrderSagaData, string>(
                context,
                message.OrderId,
                (data, orderId) => data.OrderId == orderId,
                cancellationToken);

            return Task.FromResult(sagaData);
        }
    }

    class CompleteOrder : IMessage
    {
        public string OrderId { get; set; } = string.Empty;
    }

    class OrderSagaData : ContainSagaData
    {
        public string OrderId { get; set; } = string.Empty;
    }

    #endregion
}
