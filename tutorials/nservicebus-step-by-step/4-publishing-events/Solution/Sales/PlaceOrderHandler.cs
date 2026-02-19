using Messages;
using Microsoft.Extensions.Logging;

namespace Sales;

public class PlaceOrderHandler(ILogger<PlaceOrderHandler> logger) : IHandleMessages<PlaceOrder>
{
    #region UpdatedHandler

    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received PlaceOrder, OrderId = {OrderId}", message.OrderId);

        // This is normally where some business logic would occur

        var orderPlaced = new OrderPlaced
        {
            OrderId = message.OrderId
        };
        return context.Publish(orderPlaced);
    }

    #endregion
}
