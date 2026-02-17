namespace Intro;

using Microsoft.Extensions.Logging;

#region EmptyShippingPolicy

public class ShippingPolicy(ILogger<ShippingPolicy> logger) : IHandleMessages<OrderPlaced>, IHandleMessages<OrderBilled>
{
    public Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received OrderPlaced, OrderId = {orderId}", message.OrderId);
        return Task.CompletedTask;
    }

    public Task Handle(OrderBilled message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received OrderBilled, OrderId = {orderId}", message.OrderId);
        return Task.CompletedTask;
    }
}
#endregion
