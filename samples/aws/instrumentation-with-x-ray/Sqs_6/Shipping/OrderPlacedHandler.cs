using Events;
using NServiceBus;
using NServiceBus.Logging;

namespace Shipping;

public class OrderPlacedHandler : IHandleMessages<IOrderPlaced>
{
    private static readonly ILog log = LogManager.GetLogger<OrderPlacedHandler>();

    public async Task Handle(IOrderPlaced message, IMessageHandlerContext context)
    {
        log.Info($"Shipping order '{message.Order.OrderId}'...");

        await context.Publish<IOrderShipped>(orderShipped =>
        {
            orderShipped.CustomerId = message.CustomerId;
            orderShipped.OrderId = message.Order.OrderId;
        });

        log.Info($"Order '{message.Order.OrderId}' shipped.");
    }
}