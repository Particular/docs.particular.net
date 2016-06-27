using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Saga;

public class OrderSagaLoquacious : Saga<OrderSagaDataLoquacious>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<CompleteOrder>
{
    static ILog log = LogManager.GetLogger<OrderSagaLoquacious>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaDataLoquacious> mapper)
    {
        mapper.ConfigureMapping<StartOrder>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);
        mapper.ConfigureMapping<CompleteOrder>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);
    }

    public void Handle(StartOrder message)
    {
        Data.OrderId = message.OrderId;
        log.Info($"OrderSagaLoquacious with OrderId {Data.OrderId} received StartOrder with OrderId {message.OrderId} (Saga version: {Data.Version})");

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

        var completeOrder = new CompleteOrder
        {
            OrderId = "123"
        };
        Bus.SendLocal(completeOrder);
    }

    public void Handle(CompleteOrder message)
    {
        log.Info($"OrderSagaLoquacious with OrderId {Data.OrderId} received CompleteOrder with OrderId {message.OrderId}");
        MarkAsComplete();
    }


}
