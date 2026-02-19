using Messages;
using Microsoft.Extensions.Logging;

namespace Core;

#region PlaceOrderHandler

public class PlaceOrderHandler(ILogger<PlaceOrderHandler> logger) : IHandleMessages<PlaceOrder>
{
    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received PlaceOrder, OrderId = {OrderId}", message.OrderId);

        return Task.CompletedTask;
    }
}

#endregion