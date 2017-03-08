using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;

#region sagaPhase1

[SqlSaga(
     correlationProperty: nameof(OrderSagaData.OrderNumber)
 )]
public class OrderSaga :
    SqlSaga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>
{
    static ILog log = LogManager.GetLogger<OrderSaga>();

    protected override void ConfigureMapping(MessagePropertyMapper<OrderSagaData> mapper)
    {
        mapper.MapMessage<StartOrder>(message => message.OrderNumber);
    }

    public Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        Data.OrderNumber = message.OrderNumber;
        log.Info($"Received StartOrder message. Data.OrderNumber={Data.OrderNumber}");
        return Task.CompletedTask;
    }
}

#endregion