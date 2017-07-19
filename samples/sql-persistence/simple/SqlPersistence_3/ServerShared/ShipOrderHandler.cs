using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class ShipOrderHandler :
    IHandleMessages<ShipOrder>
{
    static ILog log = LogManager.GetLogger<ShipOrderHandler>();

    public Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        log.Info($"Order Shipped. OrderId {message.OrderId}");
        return Task.CompletedTask;
    }
}