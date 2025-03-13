using System;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;
using NServiceBus.Persistence.Sql;

public class OrderLifecycleSaga(ILogger<OrderLifecycleSaga> logger) :
    SqlSaga<OrderLifecycleSagaData>,
    IAmStartedByMessages<OrderSubmitted>,
    IHandleTimeouts<OrderLifecycleSaga.OrderTimeout>
{
   
    protected override void ConfigureMapping(IMessagePropertyMapper mapper)
    {
        mapper.ConfigureMapping<OrderSubmitted>(m => m.OrderId);
    }

    protected override string CorrelationPropertyName => nameof(OrderLifecycleSagaData.OrderId);

    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        var orderTimeout = new OrderTimeout();
        await RequestTimeout(context, TimeSpan.FromSeconds(5), orderTimeout);

        var orderAccepted = new OrderAccepted
        {
            OrderId = message.OrderId,
        };

        logger.LogInformation($"Order process {message.OrderId} started.");

        await context.Reply(orderAccepted);
    }

    public Task Timeout(OrderTimeout state, IMessageHandlerContext context)
    {
        var completeOrder = new CompleteOrder
        {
            OrderId = Data.OrderId
        };
        return context.SendLocal(completeOrder);
    }

    public class OrderTimeout
    {
    }
}