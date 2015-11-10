using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Saga;

public class OrderSagaAttributes : Saga<OrderSagaDataAttributes>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<CompleteOrder>
{
    static ILog logger = LogManager.GetLogger<OrderSagaAttributes>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaDataAttributes> mapper)
    {
        mapper.ConfigureMapping<StartOrder>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);
        mapper.ConfigureMapping<CompleteOrder>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);
    }

    public void Handle(StartOrder message)
    {
        Data.OrderId = message.OrderId;
        logger.InfoFormat("OrderSagaAttributes with OrderId {0} received StartOrder with OrderId {1} (Saga version: {2})", Data.OrderId, message.OrderId, Data.Version);

        if (Data.From == null)
        {
            Data.From = new OrderSagaDataAttributes.Location();
        }
        if (Data.To == null)
        {
            Data.To = new OrderSagaDataAttributes.Location();
        }

        Data.From.Lat = 51.9166667;
        Data.From.Long = 4.5;

        Data.To.Lat = 51.51558;
        Data.To.Long = -0.12085;
    }

    public void Handle(CompleteOrder message)
    {
        logger.InfoFormat("OrderSagaAttributes with OrderId {0} received CompleteOrder with OrderId {1}", Data.OrderId, message.OrderId);
        MarkAsComplete();
    }


}
