using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderSagaLoquacious : Saga<OrderSagaDataLoquacious>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<CompleteOrder>
{
    static ILog logger = LogManager.GetLogger<OrderSagaLoquacious>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaDataLoquacious> mapper)
    {
        mapper.ConfigureMapping<StartOrder>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);
        mapper.ConfigureMapping<CompleteOrder>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);
    }

    public Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        Data.OrderId = message.OrderId;
        logger.InfoFormat("OrderSagaLoquacious with OrderId {0} received StartOrder with OrderId {1} (Saga version: {2})", Data.OrderId, message.OrderId, Data.Version);

        if (Data.From == null)
        {
            Data.From = new OrderSagaDataLoquacious.Location();
        }
        if (Data.To == null)
        {
            Data.To = new OrderSagaDataLoquacious.Location();
        }

        Data.From.Lat = 51.9166667;
        Data.From.Long = 4.5;

        Data.To.Lat = 51.51558;
        Data.To.Long = -0.12085;
        return Task.FromResult(0);
    }

    public Task Handle(CompleteOrder message, IMessageHandlerContext context)
    {
        logger.InfoFormat("OrderSagaLoquacious with OrderId {0} received CompleteOrder with OrderId {1}", Data.OrderId, message.OrderId);
        MarkAsComplete();
        return Task.FromResult(0);
    }

}
