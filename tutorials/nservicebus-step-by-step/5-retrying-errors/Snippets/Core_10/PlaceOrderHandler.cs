using Microsoft.Extensions.Logging;

namespace Core;

public class PlaceOrderHandler(ILogger<PlaceOrderHandler> logger) : IHandleMessages<PlaceOrder>
{
    #region ThrowSystemic
    public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received PlaceOrder, OrderId = {OrderId}", message.OrderId);

        // This is normally where some business logic would occur

        var orderPlaced = new OrderPlaced
        {
            OrderId = message.OrderId
        };
        await context.Publish(orderPlaced);

        throw new Exception("BOOM");
    }
    #endregion

    public void ThrowTransient()
    {
        #region ThrowTransient
        if (Random.Shared.Next(0, 5) == 0)
        {
            throw new Exception("Oops");
        }
        #endregion
    }
}