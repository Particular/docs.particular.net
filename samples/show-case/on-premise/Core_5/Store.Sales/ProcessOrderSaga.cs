using System;
using System.Diagnostics;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Saga;
using Store.Messages.Commands;
using Store.Messages.Events;

public class ProcessOrderSaga :
    Saga<ProcessOrderSaga.OrderData>,
    IAmStartedByMessages<SubmitOrder>,
    IHandleMessages<CancelOrder>,
    IHandleTimeouts<ProcessOrderSaga.BuyersRemorseIsOver>
{
    static ILog log = LogManager.GetLogger<ProcessOrderSaga>();

    public void Handle(SubmitOrder message)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        Data.OrderNumber = message.OrderNumber;
        Data.ProductIds = message.ProductIds;
        Data.ClientId = message.ClientId;

        RequestTimeout(TimeSpan.FromSeconds(20), new BuyersRemorseIsOver());
        log.Info($"Starting cool down period for order #{Data.OrderNumber}.");
    }

    public void Timeout(BuyersRemorseIsOver state)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        Bus.Publish<OrderAccepted>(e =>
        {
            e.OrderNumber = Data.OrderNumber;
            e.ProductIds = Data.ProductIds;
            e.ClientId = Data.ClientId;
        });

        MarkAsComplete();

        log.Info($"Cooling down period for order #{Data.OrderNumber} has elapsed.");
    }

    public void Handle(CancelOrder message)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        MarkAsComplete();

        Bus.Publish<OrderCancelled>(o =>
        {
            o.OrderNumber = message.OrderNumber;
            o.ClientId = message.ClientId;
        });

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
        [Unique]
        public int OrderNumber { get; set; }

        public string[] ProductIds { get; set; }
        public string ClientId { get; set; }
    }

    public class BuyersRemorseIsOver
    {
    }

}