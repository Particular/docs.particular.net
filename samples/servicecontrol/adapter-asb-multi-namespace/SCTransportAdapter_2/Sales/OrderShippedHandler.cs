using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class OrderShippedHandler :
    IHandleMessages<OrderShipped>
{
    static ILog log = LogManager.GetLogger<OrderShippedHandler>();
    ChaosGenerator chaos;

    public OrderShippedHandler(ChaosGenerator chaos)
    {
        this.chaos = chaos;
    }

    public Task Handle(OrderShipped message, IMessageHandlerContext context)
    {
        log.Info($"Completing order {message.OrderId} for {message.Value}");
        return chaos.Invoke();
    }
}