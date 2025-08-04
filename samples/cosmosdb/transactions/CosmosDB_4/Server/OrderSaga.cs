using System;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

#region thesaga

public class OrderSaga(ILogger<OrderSaga> logger) :
    Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<OrderShipped>,
    IHandleTimeouts<CompleteOrder>
{

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
        logger.LogInformation($"Received StartOrder message {Data.OrderId}. Starting Saga");

        var shipOrder = new ShipOrder
        {
            OrderId = message.OrderId
        };

        logger.LogInformation("Order will complete in 5 seconds");
        var timeoutData = new CompleteOrder
        {
            OrderDescription = orderDescription,
            OrderId = Data.OrderId,
        };

        return Task.WhenAll([
            context.SendLocal(shipOrder),
            RequestTimeout(context, TimeSpan.FromSeconds(5), timeoutData)
        ]);
    }

    public Task Handle(OrderShipped message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Order with OrderId {Data.OrderId} shipped on {message.ShippingDate}");
        return Task.CompletedTask;
    }

    public Task Timeout(CompleteOrder state, IMessageHandlerContext context)
    {
        logger.LogInformation($"Saga with OrderId {Data.OrderId} completed");
        MarkAsComplete();
        var orderCompleted = new OrderCompleted
        {
            OrderId = Data.OrderId
        };
        return context.Publish(orderCompleted);
    }
}

#endregion