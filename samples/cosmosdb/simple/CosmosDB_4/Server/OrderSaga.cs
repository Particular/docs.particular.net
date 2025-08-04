using System;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;


#region thesaga

public class OrderSaga(ILogger<OrderSaga> logger) :
    Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleTimeouts<CompleteOrder>
{

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId).ToMessage<StartOrder>(msg => msg.OrderId);
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
        CompleteOrder timeoutData = new()
        {
            OrderDescription = orderDescription,
        };

        return Task.WhenAll([
            context.SendLocal(shipOrder),
            RequestTimeout(context, TimeSpan.FromSeconds(5), timeoutData)
        ]);
    }

    public Task Timeout(CompleteOrder state, IMessageHandlerContext context)
    {
        logger.LogInformation($"Saga with OrderId {Data.OrderId} completed");
        MarkAsComplete();
        OrderCompleted orderCompleted = new()
        {
            OrderId = Data.OrderId
        };
        return context.Publish(orderCompleted);
    }
}

#endregion