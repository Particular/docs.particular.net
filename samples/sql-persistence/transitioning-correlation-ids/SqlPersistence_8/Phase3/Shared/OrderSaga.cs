using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region sagaPhase3

public class OrderSaga :
    Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>
{
    readonly static ILog log = LogManager.GetLogger<OrderSaga>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<StartOrder>(msg =>msg.OrderId);
    }

    public Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        Data.OrderId = message.OrderId;
        log.Info($"Received StartOrder message. OrderId={Data.OrderId}.");
        return Task.CompletedTask;
    }
}

#endregion