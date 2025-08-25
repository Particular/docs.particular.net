using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderSubmittedHandler :
    IHandleMessages<OrderSubmitted>
{
    static readonly ILog log = LogManager.GetLogger<OrderSubmittedHandler>();

    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        log.Info($"Order {message.OrderId} worth {message.Value} submitted");

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