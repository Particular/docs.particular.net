using Contracts;
using Microsoft.Extensions.Logging;

#region PlaceOrderHandler
sealed class PlaceOrderHandler(ILogger<PlaceOrderHandler> logger) : IHandleMessages<PlaceOrder>
{
    public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        await context.Reply(new OrderAccepted(message.OrderId));
        logger.LogInformation("Replied OrderAccepted {{ OrderId = {OrderId} }}", message.OrderId);

        await context.Publish(new OrderPlaced(message.OrderId, message.Product, message.Quantity));
        logger.LogInformation("Published OrderPlaced {{ OrderId = {OrderId} }}", message.OrderId);
    }
}
#endregion
