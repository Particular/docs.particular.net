using Messages;
using Microsoft.Extensions.Logging;

namespace Sales;

public class PlaceOrderHandler(ILogger<PlaceOrderHandler> logger) : IHandleMessages<PlaceOrder>
{
    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received PlaceOrder, OrderId = {OrderId}", message.OrderId);

        // This is normally where some business logic would occur

        #region ThrowTransientException
        // Uncomment to test throwing transient exceptions
        //if (Random.Shared.Next(0, 5) == 0)
        //{
        //    throw new Exception("Oops");
        //}
        #endregion

        #region ThrowFatalException
        // Uncomment to test throwing fatal exceptions
        //throw new Exception("BOOM");
        #endregion

        var orderPlaced = new OrderPlaced
        {
            OrderId = message.OrderId
        };

        logger.LogInformation("Publishing OrderPlaced, OrderId = {OrderId}", message.OrderId);

        return context.Publish(orderPlaced);
    }
}
