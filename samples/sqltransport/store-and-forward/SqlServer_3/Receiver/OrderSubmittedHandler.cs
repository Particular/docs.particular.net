using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
{
    static ILog log = LogManager.GetLogger<OrderSubmittedHandler>();
    
    public Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        log.InfoFormat("Order {0} worth {1} submitted", message.OrderId, message.Value);
        return context.Reply(new OrderAccepted
        {
            OrderId = message.OrderId,
        });
    }
}
