using Microsoft.Extensions.Logging;
using NServiceBus.Persistence.Sql;

public class OrderLifecycleSaga(ILogger<OrderLifecycleSaga> logger) :
    SqlSaga<OrderLifecycleSagaData>,
    IAmStartedByMessages<OrderSubmitted>,
    IHandleTimeouts<OrderTimeout>
{
    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        Data.OrderId = message.OrderId;

        await RequestTimeout<OrderTimeout>(context, TimeSpan.FromSeconds(5));
    }

    public Task Timeout(OrderTimeout state, IMessageHandlerContext context)
    {
        logger.LogInformation("Order {OrderId} has timed out", Data.OrderId);

        return Task.CompletedTask;
    }

    protected override void ConfigureMapping(IMessagePropertyMapper mapper)
    {
        mapper.ConfigureMapping<OrderSubmitted>(m => m.OrderId);
    }

    protected override string CorrelationPropertyName => "OrderId";
}