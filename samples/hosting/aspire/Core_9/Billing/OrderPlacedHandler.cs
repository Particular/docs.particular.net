using Messages;
using Microsoft.Extensions.Logging;

namespace Billing;

public class OrderPlacedHandler(ILogger<OrderPlacedHandler> log) : IHandleMessages<OrderPlaced>
{
    public Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        log.LogInformation($"Received OrderPlaced, OrderId = {message.OrderId} - Charging credit card...");

        var orderBilled = new OrderBilled
        {
            OrderId = message.OrderId
        };

        return context.Publish(orderBilled);
    }
}