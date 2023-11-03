using Events;
using NServiceBus;
using NServiceBus.Logging;

namespace Sales;

public class OrderPaidHandler : IHandleMessages<IOrderPaid>
{
    private static readonly ILog log = LogManager.GetLogger<OrderPaidHandler>();

    public async Task Handle(IOrderPaid message, IMessageHandlerContext context)
    {
        log.Info($"Order '{message.Order.OrderId}' was placed.");

        await context.Publish<IOrderPlaced>(orderPlaced =>
        {
            orderPlaced.CustomerId = message.CustomerId;
            orderPlaced.Order = message.Order;
        });
    }
}