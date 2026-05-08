#region OrderPlacedHandler

using Messages;
using Microsoft.Extensions.Logging;

namespace Shipping;

public class OrderPlacedHandler(ILogger<OrderPlacedHandler> logger) : IHandleMessages<OrderPlaced>
{
    public Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        logger.LogInformation("Shipping has received OrderPlaced, OrderId = {OrderId}", message.OrderId);
        return Task.CompletedTask;
    }
}

#endregion