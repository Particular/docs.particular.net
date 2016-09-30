using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderSaga :
    Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<CompleteOrder>
{
    static ILog log = LogManager.GetLogger<OrderSaga>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.ConfigureMapping<StartOrder>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);
        mapper.ConfigureMapping<CompleteOrder>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);
    }

    public Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        Data.OrderId = message.OrderId;
        log.Info($"OrderSaga with OrderId {Data.OrderId} received StartOrder with OrderId {message.OrderId} (Saga version: {Data.Version})");

        var completeOrder = new CompleteOrder
        {
            OrderId = "123"
        };
        return context.SendLocal(completeOrder);
    }

    public Task Handle(CompleteOrder message, IMessageHandlerContext context)
    {
        log.Info($"OrderSaga with OrderId {Data.OrderId} received CompleteOrder with OrderId {message.OrderId}");
        MarkAsComplete();
        return Task.CompletedTask;
    }

}
