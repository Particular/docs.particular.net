using Commands;
using NServiceBus.Logging;

namespace Sales;

public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
{
    private static readonly ILog log = LogManager.GetLogger<PlaceOrderHandler>();

    public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        log.Info($"Received order '{message.Order.OrderId}'...");
        await context.Send(new ChargeOrder
        {
            CustomerId = message.CustomerId,
            Order = message.Order
        });
    }
}