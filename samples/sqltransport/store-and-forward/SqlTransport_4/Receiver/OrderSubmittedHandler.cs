using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderSubmittedHandler :
    IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<OrderSubmittedHandler>();

    public Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        log.Info($"Order {message.OrderId} worth {message.Value} submitted");
        var orderAccepted = new OrderAccepted
        {
            OrderId = message.OrderId,
        };
        return context.Reply(orderAccepted);
    }
}
