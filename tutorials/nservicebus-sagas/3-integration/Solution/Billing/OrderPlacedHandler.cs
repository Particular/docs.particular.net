using Messages;
using Microsoft.Extensions.Logging;

namespace Billing;

public class OrderPlacedHandler(OrderCalculator orderCalculator, ILogger<OrderPlacedHandler> logger): IHandleMessages<OrderPlaced>
{
    public Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received OrderPlaced, OrderId = {OrderId} - Charging credit card...", message.OrderId);

        var orderBilled = new OrderBilled
        {
            CustomerId = message.CustomerId,
            OrderId = message.OrderId,
            OrderValue = orderCalculator.GetOrderTotal(message.OrderId)
        };
        return context.Publish(orderBilled);
    }
}