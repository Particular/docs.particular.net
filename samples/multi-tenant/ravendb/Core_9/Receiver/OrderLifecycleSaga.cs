﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class OrderLifecycleSaga(ILogger<OrderLifecycleSaga> logger) :
    Saga<OrderLifecycleSagaData>,
    IAmStartedByMessages<OrderSubmitted>,
    IHandleTimeouts<OrderTimeout>
{
 
    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        Data.OrderId = message.OrderId;

        await RequestTimeout<OrderTimeout>(context, TimeSpan.FromSeconds(5));
    }

    public Task Timeout(OrderTimeout state, IMessageHandlerContext context)
    {
        logger.LogInformation($"Order {Data.OrderId} has timed out");
        return Task.CompletedTask;
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderLifecycleSagaData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<OrderSubmitted>(m => m.OrderId);
    }
}