using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;

#region sagaPhase3

public class OrderSaga :
    SqlSaga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>
{
    static ILog log = LogManager.GetLogger<OrderSaga>();

    protected override void ConfigureMapping(IMessagePropertyMapper mapper)
    {
        mapper.ConfigureMapping<StartOrder>(_ => _.OrderId);
    }

    protected override string CorrelationPropertyName => nameof(OrderSagaData.OrderId);

    public Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        Data.OrderId = message.OrderId;
        log.Info($"Received StartOrder message. OrderId={Data.OrderId}.");
        return Task.CompletedTask;
    }
}

#endregion