using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class ShipOrderHandler :
    IHandleMessages<ShipOrder>
{
    static ILog log = LogManager.GetLogger<ShipOrderHandler>();
    ChaosGenerator chaos;

    public ShipOrderHandler(ChaosGenerator chaos)
    {
        this.chaos = chaos;
    }

    public async Task Handle(ShipOrder message, IMessageHandlerContext context)
    {
        log.Info($"Shipping order {message.OrderId} for {message.Value}");
        await chaos.Invoke()
            .ConfigureAwait(false);
        var orderShipped = new OrderShipped
        {
            OrderId = message.OrderId,
            Value = message.Value
        };
        await context.Reply(orderShipped)
            .ConfigureAwait(false);
    }
}