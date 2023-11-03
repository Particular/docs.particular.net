using Commands;
using Domain;
using Events;
using NServiceBus;
using NServiceBus.Logging;

namespace Inventory;

public class OrderPlacedHandler : IHandleMessages<IOrderPlaced>
{
    private static readonly ILog log = LogManager.GetLogger<OrderPlacedHandler>();

    public async Task Handle(IOrderPlaced message, IMessageHandlerContext context)
    {
        log.Info($"Adjust stock...");

        foreach (var orderLine in message.Order.Lines)
        {
            await context.SendLocal(new UpdateProductStock
            {
                ProductId = orderLine.Product.ProductId,
                Quantity = orderLine.Quantity,
                StockOperation = StockOperation.Decrease
            });
        }
    }
}