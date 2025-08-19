using NServiceBus.Logging;

public class OrderSubmittedHandler :
    IHandleMessages<OrderSubmitted>
{
    static readonly ILog log = LogManager.GetLogger<OrderSubmittedHandler>();

    public Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        log.Info($"Order {message.OrderId} worth {message.Value} submitted");

        var order = new Order
        {
            OrderId = message.OrderId,
            Value = message.Value
        };
        var session = context.SynchronizedStorageSession.Session();
        session.Save(order);

        var orderAccepted = new OrderAccepted
        {
            OrderId = message.OrderId,
        };
        return context.Reply(orderAccepted);
    }
}