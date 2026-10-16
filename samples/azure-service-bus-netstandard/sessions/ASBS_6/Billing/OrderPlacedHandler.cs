using Shared;

namespace Billing;

public class OrderPlacedHandler(ILogger<OrderPlacedHandler> logger) : IHandleMessages<OrderPlaced>
{
    public Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        logger.LogInformation("Processing OrderPlaced, payment captured for {order}", message.OrderId);
        
        return Task.CompletedTask;
    }
}