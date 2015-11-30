using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using Store.Messages.Commands;
using Store.Messages.Events;

public class ProcessOrderSaga : Saga<ProcessOrderSaga.OrderData>,
                                IAmStartedByMessages<SubmitOrder>,
                                IHandleMessages<CancelOrder>,
                                IHandleTimeouts<ProcessOrderSaga.BuyersRemorseIsOver>
{
    public async Task Handle(SubmitOrder message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        Data.OrderNumber = message.OrderNumber;
        Data.ProductIds = message.ProductIds;
        Data.ClientId = message.ClientId;

        await RequestTimeout(context, TimeSpan.FromSeconds(20), new BuyersRemorseIsOver());
        Console.WriteLine("Starting cool down period for order #{0}.", Data.OrderNumber);
    }

    public async Task Timeout(BuyersRemorseIsOver state, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        await context.Publish<OrderAccepted>(e =>
            {
                e.OrderNumber = Data.OrderNumber;
                e.ProductIds = Data.ProductIds;
                e.ClientId = Data.ClientId;
            });

        MarkAsComplete();

        Console.WriteLine("Cooling down period for order #{0} has elapsed.", Data.OrderNumber);
    }

    public async Task Handle(CancelOrder message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
                Debugger.Break();
        }

        MarkAsComplete();

        await context.Publish<OrderCancelled>(o =>
            {
                o.OrderNumber = message.OrderNumber;
                o.ClientId = message.ClientId;
            });

        Console.WriteLine("Order #{0} was cancelled.", message.OrderNumber);
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderData> mapper)
    {
        mapper.ConfigureMapping<SubmitOrder>(message => message.OrderNumber)
            .ToSaga(sagaData => sagaData.OrderNumber);
        mapper.ConfigureMapping<CancelOrder>(message => message.OrderNumber)
            .ToSaga(sagaData => sagaData.OrderNumber);
    }

    public class OrderData : ContainSagaData
    {
        public int OrderNumber { get; set; }
        public string[] ProductIds { get; set; }
        public string ClientId { get; set; }
    }

    public class BuyersRemorseIsOver
    {
    }
    
}
