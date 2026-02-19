using Messages;
using Microsoft.Extensions.Logging;

namespace Sales;

public class PlaceOrderHandler(ILogger<PlaceOrderHandler> logger) : IHandleMessages<PlaceOrder>
{
    public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received PlaceOrder, OrderId = {orderId}", message.OrderId);

        // This is normally where some business logic would occur

        var orderPlaced = new OrderPlaced
        {
            OrderId = message.OrderId
        };
        await context.Publish(orderPlaced);

        // Uncomment to test throwing a systemic exception
        //throw new Exception("BOOM");

        // Uncomment to test throwing a transient exception
        //if (Random.Shared.Next(0, 5) == 0)
        //{
        //    throw new Exception("Oops");
        //}
    }
}