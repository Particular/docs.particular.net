﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Store.Messages.Commands;
using Store.Messages.Events;

public class ProcessOrderSaga :
    Saga<ProcessOrderSaga.OrderData>,
    IAmStartedByMessages<SubmitOrder>,
    IHandleMessages<CancelOrder>,
    IHandleTimeouts<ProcessOrderSaga.BuyersRemorseIsOver>
{
    static ILog log = LogManager.GetLogger<ProcessOrderSaga>();

    public Task Handle(SubmitOrder message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        Data.OrderNumber = message.OrderNumber;
        Data.ProductIds = string.Join(";", message.ProductIds);
        Data.ClientId = message.ClientId;

        log.Info($"Starting cool down period for order #{Data.OrderNumber}.");
        return RequestTimeout(context, TimeSpan.FromSeconds(20), new BuyersRemorseIsOver());
    }

    public async Task Timeout(BuyersRemorseIsOver state, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        var orderAccepted = new OrderAccepted
        {
            OrderNumber = Data.OrderNumber,
            ProductIds = Data.ProductIds.Split(';'),
            ClientId = Data.ClientId
        };
        await context.Publish(orderAccepted)
            .ConfigureAwait(false);

        MarkAsComplete();

        log.Info($"Cooling down period for order #{Data.OrderNumber} has elapsed.");
    }

    public async Task Handle(CancelOrder message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        MarkAsComplete();

        var orderCancelled = new OrderCancelled
        {
            OrderNumber = message.OrderNumber,
            ClientId = message.ClientId
        };
        await context.Publish(orderCancelled)
            .ConfigureAwait(false);

        log.Info($"Order #{message.OrderNumber} was cancelled.");
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderData> mapper)
    {
        mapper.ConfigureMapping<SubmitOrder>(message => message.OrderNumber)
            .ToSaga(sagaData => sagaData.OrderNumber);
        mapper.ConfigureMapping<CancelOrder>(message => message.OrderNumber)
            .ToSaga(sagaData => sagaData.OrderNumber);
    }

    public class OrderData :
        ContainSagaData
    {
        public int OrderNumber { get; set; }
        public string ProductIds { get; set; }
        public string ClientId { get; set; }
    }

    public class BuyersRemorseIsOver
    {
    }

}