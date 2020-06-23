using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region sagaPhase1

public class OrderSaga :
    Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>
{
    static ILog log = LogManager.GetLogger<OrderSaga>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.ConfigureMapping<StartOrder>(msg => msg.OrderNumber).ToSaga(saga => saga.OrderNumber);
    }

    public Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        Data.OrderNumber = message.OrderNumber;
        log.Info($"Received StartOrder message. Data.OrderNumber={Data.OrderNumber}");
        return Task.CompletedTask;
    }
}

#endregion