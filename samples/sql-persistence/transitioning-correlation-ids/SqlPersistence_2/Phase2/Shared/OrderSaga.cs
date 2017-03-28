using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;

#region sagaPhase2

public class OrderSaga :
    SqlSaga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>
{
    static ILog log = LogManager.GetLogger<OrderSaga>();

    protected override void ConfigureMapping(IMessagePropertyMapper mapper)
    {
        mapper.ConfigureMapping<StartOrder>(_ => _.OrderNumber);
    }

    protected override string CorrelationPropertyName => nameof(OrderSagaData.OrderNumber);
    protected override string TransitionalCorrelationPropertyName => nameof(OrderSagaData.OrderId);

    public Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        Data.OrderId = message.OrderId;
        Data.OrderNumber = message.OrderNumber;
        log.Info($"Received StartOrder message. OrderId={Data.OrderId}. OrderNumber={Data.OrderNumber}");
        return Task.CompletedTask;
    }
}

#endregion