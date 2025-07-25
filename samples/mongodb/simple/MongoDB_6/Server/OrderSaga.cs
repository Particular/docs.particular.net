﻿using Microsoft.Extensions.Logging;
#region thesaga

public class OrderSaga(ILogger<OrderSaga> logger) :
    Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleTimeouts<CompleteOrder>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.MapSaga(sagaData => sagaData.OrderId)
            .ToMessage<StartOrder>(message => message.OrderId);
    }

    public Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        Data.OrderId = message.OrderId;
        var orderDescription = $"The saga for order {message.OrderId}";
        Data.OrderDescription = orderDescription;

        logger.LogInformation("Received StartOrder message {OrderId}. Starting Saga", Data.OrderId);
        logger.LogInformation("Order will complete in 5 seconds");

        var timeoutData = new CompleteOrder
        {
            OrderDescription = orderDescription
        };

        return RequestTimeout(context, TimeSpan.FromSeconds(5), timeoutData);
    }

    public Task Timeout(CompleteOrder state, IMessageHandlerContext context)
    {
        logger.LogInformation("Saga with OrderId {OrderId} completed", Data.OrderId);

        var orderCompleted = new OrderCompleted
        {
            OrderId = Data.OrderId
        };

        MarkAsComplete();

        return context.Publish(orderCompleted);
    }
}

#endregion