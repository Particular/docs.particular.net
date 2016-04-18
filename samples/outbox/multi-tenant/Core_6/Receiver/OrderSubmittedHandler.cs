using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<OrderSubmittedHandler>();

    public Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        log.Info($"Order {message.OrderId} worth {message.Value} submitted");

        context.SynchronizedStorageSession.Session().Save(new Order
        {
            OrderId = message.OrderId,
            Value = message.Value
        });

        return context.Reply(new OrderAccepted
        {
            OrderId = message.OrderId,
        });
    }
}