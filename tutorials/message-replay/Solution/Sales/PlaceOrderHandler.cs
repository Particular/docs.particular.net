using Messages;
using Microsoft.Extensions.Logging;

#pragma warning disable CS0162 // Unreachable code detected

namespace Sales;

public class PlaceOrderHandler(ILogger<PlaceOrderHandler> logger) : IHandleMessages<PlaceOrder>
{

    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received PlaceOrder, OrderId = {OrderId}", message.OrderId);

        // This is normally where some business logic would occur

        // Uncomment to test throwing a systemic exception
        //throw new Exception("BOOM");

        var orderPlaced = new OrderPlaced
        {
            OrderId = message.OrderId
        };
        return context.Publish(orderPlaced);
    }
}

#pragma warning restore CS0162 // Unreachable code detected