using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region thesaga

public class OrderSaga : Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleTimeouts<CompleteOrder>
{
    static ILog logger = LogManager.GetLogger<OrderSaga>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.ConfigureMapping<StartOrder>(message => message.OrderId)
            .ToSaga(sagaData => sagaData.OrderId);
    }

    public async Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        Data.OrderId = message.OrderId;
        var orderDescription = "The saga for order " + message.OrderId;
        Data.OrderDescription = orderDescription;
        logger.InfoFormat($"Received StartOrder message {Data.OrderId}. Starting Saga");

        var shipOrder = new ShipOrder
        {
            OrderId = message.OrderId
        };
        await context.SendLocal(shipOrder)
            .ConfigureAwait(false);

        logger.Info("Order will complete in 5 seconds");
        var timeoutData = new CompleteOrder
        {
            OrderDescription = orderDescription
        };

        await RequestTimeout(context, TimeSpan.FromSeconds(5), timeoutData)
            .ConfigureAwait(false);
    }

    public async Task Timeout(CompleteOrder state, IMessageHandlerContext context)
    {
        logger.InfoFormat($"Saga with OrderId {Data.OrderId} completed");
        var orderCompleted = new OrderCompleted
        {
            OrderId = Data.OrderId
        };
        await context.Publish(orderCompleted)
            .ConfigureAwait(false);
        MarkAsComplete();
    }
}

#endregion