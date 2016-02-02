using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Saga;

public class OrdersSaga : Saga<OrdersSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<IncrementOrder>
{
    static ILog logger = LogManager.GetLogger<OrdersSaga>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrdersSagaData> mapper)
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

