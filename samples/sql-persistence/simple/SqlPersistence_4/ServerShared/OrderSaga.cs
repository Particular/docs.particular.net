using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;

#region thesaga

public class OrderSaga :
    SqlSaga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleTimeouts<CompleteOrder>
{
    static ILog log = LogManager.GetLogger<OrderSaga>();

    protected override void ConfigureMapping(IMessagePropertyMapper mapper)
    {
        mapper.ConfigureMapping<StartOrder>(_ => _.OrderId);
    }

    protected override string CorrelationPropertyName => nameof(OrderSagaData.OrderId);

    public Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        var orderDescription = $"The saga for order {message.OrderId}";
        Data.OrderDescription = orderDescription;
        log.Info($"Received StartOrder message {Data.OrderId}. Starting Saga");

        var shipOrder = new ShipOrder
        {
            OrderId = message.OrderId
        };

        log.Info("Order will complete in 5 seconds");
        var timeoutData = new CompleteOrder
        {
            OrderDescription = orderDescription
        };

        return Task.WhenAll(
            context.SendLocal(shipOrder),
            RequestTimeout(context, TimeSpan.FromSeconds(5), timeoutData)
        );
    }

    public Task Timeout(CompleteOrder state, IMessageHandlerContext context)
    {
        log.Info($"Saga with OrderId {Data.OrderId} completed");
        MarkAsComplete();
        var orderCompleted = new OrderCompleted
        {
            OrderId = Data.OrderId
        };
        return context.Publish(orderCompleted);
    }
}

#endregion