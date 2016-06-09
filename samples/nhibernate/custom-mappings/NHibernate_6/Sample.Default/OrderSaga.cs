using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Saga;

public class OrderSaga : Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<CompleteOrder>
{
    static ILog logger = LogManager.GetLogger<OrderSaga>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.ConfigureMapping<StartOrder>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);
        mapper.ConfigureMapping<CompleteOrder>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);
    }

    public void Handle(StartOrder message)
    {
        Data.OrderId = message.OrderId;
        logger.InfoFormat("OrderSaga with OrderId {0} received StartOrder with OrderId {1} (Saga version: {2})", Data.OrderId, message.OrderId, Data.Version);

        var completeOrder = new CompleteOrder
        {
            OrderId = "123"
        };
        Bus.SendLocal(completeOrder);
    }

    public void Handle(CompleteOrder message)
    {
        logger.InfoFormat("OrderSaga with OrderId {0} received CompleteOrder with OrderId {1}", Data.OrderId, message.OrderId);
        MarkAsComplete();
    }

}