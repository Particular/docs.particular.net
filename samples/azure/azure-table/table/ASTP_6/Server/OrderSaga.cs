﻿using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

#region theordersaga

public class OrderSaga(ILogger<OrderSaga> logger) :
    Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<CompleteOrder>
{

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


        logger.LogInformation("Received StartOrder message {OrderId}. Starting Saga", Data.OrderId);

        var shipOrder = new ShipOrder
        {
            OrderId = message.OrderId
        };

        return context.SendLocal(shipOrder);
    }

    public Task Handle(CompleteOrder message, IMessageHandlerContext context)
    {

        logger.LogInformation("Saga with OrderId {OrderId} completed", Data.OrderId);
        MarkAsComplete();
        var orderCompleted = new OrderCompleted
        {
            OrderId = Data.OrderId
        };
        return context.Publish(orderCompleted);
    }
}

#endregion