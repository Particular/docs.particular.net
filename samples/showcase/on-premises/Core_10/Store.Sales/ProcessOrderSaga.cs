using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;
using Store.Messages.Commands;
using Store.Messages.Events;

public class ProcessOrderSaga(ILogger<ProcessOrderSaga> logger) :
    Saga<ProcessOrderSaga.OrderData>,
    IAmStartedByMessages<SubmitOrder>,
    IHandleMessages<CancelOrder>,
    IHandleTimeouts<ProcessOrderSaga.BuyersRemorseIsOver>
{

    public Task Handle(SubmitOrder message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        Data.OrderNumber = message.OrderNumber;
        Data.ProductIds = message.ProductIds;
        Data.ClientId = message.ClientId;

        logger.LogInformation("Starting cool down period for order #{OrderNumber}.", Data.OrderNumber);
        return RequestTimeout(context, TimeSpan.FromSeconds(20), new BuyersRemorseIsOver());
    }

    public Task Timeout(BuyersRemorseIsOver state, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        MarkAsComplete();

        logger.LogInformation("Cooling down period for order #{OrderNumber} has elapsed.", Data.OrderNumber);

        var orderAccepted = new OrderAccepted
        {
            OrderNumber = Data.OrderNumber,
            ProductIds = Data.ProductIds,
            ClientId = Data.ClientId
        };
        return context.Publish(orderAccepted);
    }

    public Task Handle(CancelOrder message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        logger.LogInformation("Order #{OrderNumber} was cancelled.", message.OrderNumber);

        MarkAsComplete();

        var orderCancelled = new OrderCancelled
        {
            OrderNumber = message.OrderNumber,
            ClientId = message.ClientId
        };
        return context.Publish(orderCancelled);
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderData> mapper)
    {
        mapper.MapSaga(sagaData => sagaData.OrderNumber)
            .ToMessage<SubmitOrder>(message => message.OrderNumber)
            .ToMessage<CancelOrder>(message => message.OrderNumber);
    }

    public class OrderData :
        ContainSagaData
    {
        public int OrderNumber { get; set; }
        public string[] ProductIds { get; set; }
        public string ClientId { get; set; }
    }

    public class BuyersRemorseIsOver
    {
    }

}