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

    public Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        Data.OrderId = message.OrderId;
        string orderDescription = "The saga for order " + message.OrderId;
        Data.OrderDescription = orderDescription;
        logger.InfoFormat("Received StartOrder message {0}. Starting Saga", Data.OrderId);

        context.SendLocal(new ShipOrder
        {
            OrderId = message.OrderId
        });

        logger.Info("Order will complete in 5 seconds");
        CompleteOrder timeoutData = new CompleteOrder
        {
            OrderDescription = orderDescription
        };
        return RequestTimeout(context, TimeSpan.FromSeconds(5), timeoutData);
    }

    public Task Timeout(CompleteOrder state, IMessageHandlerContext context)
    {
        logger.InfoFormat("Saga with OrderId {0} completed", Data.OrderId);
        context.Publish(new OrderCompleted
        {
            OrderId = Data.OrderId
        });
        MarkAsComplete();
        return Task.FromResult(0);
    }
}

#endregion