using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region theordersaga

public class OrderSaga :
    Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<CompleteOrder>
{
    static ILog log = LogManager.GetLogger<OrderSaga>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<StartOrder>(msg => msg.OrderId)
            .ToMessage<CompleteOrder>(msg => msg.OrderId);
    }

    public Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        Data.OrderId = message.OrderId;
        Data.OrderDescription = $"The saga for order {message.OrderId}";

        log.Info($"Received StartOrder message {Data.OrderId}. Starting Saga");

        var shipOrder = new ShipOrder
        {
            OrderId = message.OrderId
        };

        return context.SendLocal(shipOrder);
    }

    public Task Handle(CompleteOrder message, IMessageHandlerContext context)
    {
        log.Info($"Saga with OrderId {Data.OrderId} completed");
        MarkAsComplete();
        var orderCompleted = new OrderCompleted
        {
            OrderId = Data.OrderId
        };
        return context.Publish(orderCompleted);
    }
}

#endregion