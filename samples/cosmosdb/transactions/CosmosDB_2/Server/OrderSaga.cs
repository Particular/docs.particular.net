using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region thesaga

public class OrderSaga :
    Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<OrderShipped>,
    IHandleTimeouts<CompleteOrder>
{
    static readonly ILog Log = LogManager.GetLogger<OrderSaga>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<StartOrder>(msg => msg.OrderId)
            .ToMessageHeader<OrderShipped>("Sample.CosmosDB.Transaction.OrderId");
    }

    public Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        var orderDescription = $"The saga for order {message.OrderId}";
        Data.OrderDescription = orderDescription;
        Log.Info($"Received StartOrder message {Data.OrderId}. Starting Saga");

        var shipOrder = new ShipOrder
        {
            OrderId = message.OrderId
        };

        Log.Info("Order will complete in 5 seconds");
        var timeoutData = new CompleteOrder
        {
            OrderDescription = orderDescription,
            OrderId = Data.OrderId,
        };

        return Task.WhenAll(
            context.SendLocal(shipOrder),
            RequestTimeout(context, TimeSpan.FromSeconds(5), timeoutData)
        );
    }

    public Task Handle(OrderShipped message, IMessageHandlerContext context)
    {
        Log.Info($"Order with OrderId {Data.OrderId} shipped on {message.ShippingDate}");
        return Task.CompletedTask;
    }

    public Task Timeout(CompleteOrder state, IMessageHandlerContext context)
    {
        Log.Info($"Saga with OrderId {Data.OrderId} completed");
        MarkAsComplete();
        var orderCompleted = new OrderCompleted
        {
            OrderId = Data.OrderId
        };
        return context.Publish(orderCompleted);
    }
}

#endregion