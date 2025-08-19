using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;

#region sagaPhase2

[SqlSaga(transitionalCorrelationProperty: nameof(OrderSagaData.OrderId))]
public class OrderSaga :
    Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>
{
    readonly static ILog log = LogManager.GetLogger<OrderSaga>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderNumber)
            .ToMessage<StartOrder>(msg => msg.OrderNumber);
    }

    public Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        Data.OrderId = message.OrderId;
        Data.OrderNumber = message.OrderNumber;
        log.Info($"Received StartOrder message. OrderId={Data.OrderId}. OrderNumber={Data.OrderNumber}");
        return Task.CompletedTask;
    }
}

#endregion