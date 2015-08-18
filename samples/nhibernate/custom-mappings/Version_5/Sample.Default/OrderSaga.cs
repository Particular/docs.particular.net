using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Saga;

public class OrderSaga : Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<CompleteOrder>
{
    static ILog logger = LogManager.GetLogger(typeof(OrderSaga));

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.ConfigureMapping<StartOrder>(m => m.OrderId)
                .ToSaga(s => s.OrderId);
        mapper.ConfigureMapping<CompleteOrder>(m => m.OrderId)
                .ToSaga(s => s.OrderId);
    }

    public void Handle(StartOrder message)
    {
        Data.OrderId = message.OrderId;
        logger.Info(string.Format("OrderSaga with OrderId {0} received StartOrder with OrderId {1} (Saga version: {2})", Data.OrderId, message.OrderId, Data.Version));
    }

    public void Handle(CompleteOrder message)
    {
        logger.Info(string.Format("OrderSaga with OrderId {0} received CompleteOrder with OrderId {1}", Data.OrderId, message.OrderId));
        MarkAsComplete();
    }

}
