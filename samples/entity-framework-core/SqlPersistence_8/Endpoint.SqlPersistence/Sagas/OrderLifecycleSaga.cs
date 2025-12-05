using System;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;
using NServiceBus.Persistence.Sql;

public class OrderLifecycleSaga(ILogger<OrderLifecycleSaga> logger) :
    Saga<OrderLifecycleSagaData>,
    IAmStartedByMessages<OrderSubmitted>,
    IHandleTimeouts<OrderLifecycleSaga.OrderTimeout>
{

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderLifecycleSagaData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<OrderSubmitted>(msg => msg.OrderId);
    }

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