using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;

#region sagaPhase3

[SqlSaga(
     correlationProperty: nameof(OrderSagaData.OrderId)
 )]
public class OrderSaga :
    SqlSaga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>
{
    static ILog log = LogManager.GetLogger<OrderSaga>();

    protected override void ConfigureMapping(MessagePropertyMapper<OrderSagaData> mapper)
    {
        mapper.MapMessage<StartOrder>(_ => _.OrderId);
    }

    public Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        Data.OrderId = message.OrderId;
        log.Info($"Received StartOrder message. OrderId={Data.OrderId}.");
        return Task.CompletedTask;
    }
}

#endregion