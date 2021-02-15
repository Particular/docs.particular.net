using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class PlaceOrderHandler :
    IHandleMessages<PlaceOrder>
{
    static ILog log = LogManager.GetLogger<PlaceOrderHandler>();

    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        log.Info($"Order {message.OrderId} placed");
        return Task.CompletedTask;
    }
}