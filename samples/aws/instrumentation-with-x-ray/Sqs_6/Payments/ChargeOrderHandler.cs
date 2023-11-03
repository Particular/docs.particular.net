using Commands;
using Events;
using NServiceBus;
using NServiceBus.Logging;

namespace Finance;

public class ChargeOrderHandler : IHandleMessages<ChargeOrder>
{
    private static readonly ILog log = LogManager.GetLogger<ChargeOrderHandler>();

    public async Task Handle(ChargeOrder message, IMessageHandlerContext context)
    {
        log.Info($"Charging credit card for order '{message.Order.OrderId}'...");

        await context.Publish<IOrderPaid>(orderCharged =>
        {
            orderCharged.Order = message.Order;
        });

        log.Info($"Order '{message.Order.OrderId}' was successfully charged to the credit card.");
    }
}