using Messages;
using Microsoft.Extensions.Logging;

namespace Billing;

public class OrderPlacedHandler(ILogger<OrderPlacedHandler> logger) : IHandleMessages<OrderPlaced>
{
    public Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        logger.LogInformation("Billing has received OrderPlaced, OrderId = {OrderId}", message.OrderId);
        return Task.CompletedTask;
    }
}