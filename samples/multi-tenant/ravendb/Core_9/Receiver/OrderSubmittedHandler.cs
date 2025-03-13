using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class OrderSubmittedHandler(ILogger<OrderSubmittedHandler> logger) :
    IHandleMessages<OrderSubmitted>
{   
    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Order {message.OrderId} worth {message.Value} submitted");

        var order = new Order
        {
            Id = message.OrderId,
            Value = message.Value
        };
        var session = context.SynchronizedStorageSession.RavenSession();

        await session.StoreAsync(order, context.CancellationToken);

        var orderAccepted = new OrderAccepted
        {
            OrderId = message.OrderId,
        };
        await context.Reply(orderAccepted);
    }
}