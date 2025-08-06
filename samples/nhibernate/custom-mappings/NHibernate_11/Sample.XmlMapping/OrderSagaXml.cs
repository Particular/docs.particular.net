using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderSagaXml :
    Saga<OrderSagaDataXml>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<CompleteOrder>
{
    static ILog log = LogManager.GetLogger<OrderSagaXml>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaDataXml> mapper)
    {
        mapper.MapSaga(sagaData => sagaData.OrderId)
            .ToMessage<StartOrder>(message => message.OrderId)
            .ToMessage<CompleteOrder>(message => message.OrderId);
    }

    public Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        Data.OrderId = message.OrderId;
        log.Info($"Saga with OrderId {Data.OrderId} received StartOrder with OrderId {message.OrderId} (Saga version: {Data.Version})");

        Data.From ??= new OrderSagaDataXml.Location();
        Data.To ??= new OrderSagaDataXml.Location();

        Data.From.Lat = 51.9166667;
        Data.From.Long = 4.5;

        Data.To.Lat = 51.51558;
        Data.To.Long = -0.12085;

        var completeOrder = new CompleteOrder
        {
            OrderId = "123"
        };
        return context.SendLocal(completeOrder);
    }

    public Task Handle(CompleteOrder message, IMessageHandlerContext context)
    {
        log.Info($"Saga with OrderId {Data.OrderId} received CompleteOrder with OrderId {message.OrderId}");
        MarkAsComplete();
        return Task.CompletedTask;
    }
}
