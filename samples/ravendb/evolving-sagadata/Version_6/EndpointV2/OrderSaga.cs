using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderSaga : Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<IncrementOrder>
{
    static ILog logger = LogManager.GetLogger<OrderSaga>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.ConfigureMapping<StartOrder>(message => message.OrderId)
            .ToSaga(sagaData => sagaData.OrderId);
        mapper.ConfigureMapping<IncrementOrder>(message => message.OrderId)
            .ToSaga(sagaData => sagaData.OrderId);
    }

    public Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        Data.OrderId = message.OrderId;
        Data.NumberOfItems = message.ItemCount;
        logger.InfoFormat("Received StartOrder message with OrderId:{0}", Data.OrderId);
        logger.InfoFormat("Saga NumberOfItems is now {0}", Data.NumberOfItems);

        return Task.FromResult(false);
    }

    public Task Handle(IncrementOrder message, IMessageHandlerContext context)
    {
        logger.InfoFormat("Received IncrementOrder message with OrderId:{0}", Data.OrderId);
        Data.NumberOfItems += 1;
        logger.InfoFormat("NumberOfItems is now {0}", Data.NumberOfItems);

        return Task.FromResult(false);
    }
}
