using Shared;

namespace Inventory;

public class OrderPlacedHandler(ILogger<OrderPlacedHandler> logger) : IHandleMessages<OrderPlaced>
{
    public Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        logger.LogInformation("Processing OrderPlaced, inventory reserved for {order}", message.OrderId);
        
        return Task.CompletedTask;
    }
}