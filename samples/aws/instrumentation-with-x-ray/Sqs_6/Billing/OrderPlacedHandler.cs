using Events;
using NServiceBus;
using NServiceBus.Logging;

namespace Billing;

public class OrderPlacedEventHandler : IHandleMessages<IOrderPlaced>
{
    private static readonly ILog log = LogManager.GetLogger<OrderPlacedEventHandler>();

    public async Task Handle(IOrderPlaced message, IMessageHandlerContext context)
    {
        log.Info($"Billing order '{message.Order.OrderId}'...");

        await context.Publish<IOrderBilled>(orderBilled =>
        {
            orderBilled.CustomerId = message.CustomerId;
            orderBilled.OrderId = message.Order.OrderId;
        });

        log.Info($"Order '{message.Order.OrderId}' billed.");
    }
}