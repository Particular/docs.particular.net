using System;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Saga;

public class OrderSagaLoquacious : Saga<OrderSagaDataLoquacious>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<CompleteOrder>,
    IHandleTimeouts<CancelOrder>
{
    static ILog logger = LogManager.GetLogger(typeof(OrderSagaLoquacious));

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaDataLoquacious> mapper)
    {
        mapper.ConfigureMapping<StartOrder>(m => m.OrderId)
                .ToSaga(s => s.OrderId);
        mapper.ConfigureMapping<CompleteOrder>(m => m.OrderId)
                .ToSaga(s => s.OrderId);
    }

    public void Handle(StartOrder message)
    {
        Data.OrderId = message.OrderId;
        logger.Info(string.Format("Saga with OrderId {0} received StartOrder with OrderId {1} (Saga version: {2})", Data.OrderId, message.OrderId, Data.Version));
        RequestTimeout<CancelOrder>(TimeSpan.FromSeconds(10));
    }

    public void Handle(CompleteOrder message)
    {
        logger.Info(string.Format("Saga with OrderId {0} received CompleteOrder with OrderId {1}", Data.OrderId, message.OrderId));
        MarkAsComplete();
    }

    public void Timeout(CancelOrder state)
    {
        logger.Info(string.Format("Complete not received soon enough OrderId {0}", Data.OrderId));
        MarkAsComplete();
    }

}
