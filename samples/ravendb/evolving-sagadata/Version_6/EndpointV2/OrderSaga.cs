using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Saga;

public class OrderSaga : Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<IncrementOrder>
{
    static ILog logger = LogManager.GetLogger<OrderSaga>();
    IBus bus;

    public OrderSaga(IBus bus)
    {
        this.bus = bus;
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.ConfigureMapping<StartOrder>(message => message.OrderId)
            .ToSaga(sagaData => sagaData.OrderId);
        mapper.ConfigureMapping<IncrementOrder>(message => message.OrderId)
            .ToSaga(sagaData => sagaData.OrderId);
    }

    public void Handle(StartOrder message)
    {
        Data.OrderId = message.OrderId;
        Data.NumberOfItems = message.ItemCount;
        logger.InfoFormat("Received StartOrder message with OrderId:{0}", Data.OrderId);
        logger.InfoFormat("Saga NumberOfItems is now {0}", Data.NumberOfItems);
    }

    public void Handle(IncrementOrder message)
    {
        logger.InfoFormat("Received IncrementOrder message with OrderId:{0}", Data.OrderId);
        Data.NumberOfItems += 1;
        logger.InfoFormat("NumberOfItems is now {0}", Data.NumberOfItems);
    }

}
