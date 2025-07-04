using Microsoft.Extensions.Logging;

public class OrderDelayedHandler(ILogger<OrderDelayedHandler> logger) : IHandleMessages<OrderDelayed>
{
    public Task Handle(OrderDelayed message, IMessageHandlerContext context)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var coupon = new string(
            Enumerable.Repeat(chars, 8)
                .Select(s => s[Random.Shared.Next(s.Length)])
                .ToArray());

        logger.LogInformation("Order {OrderId} is slightly delayed. We are sorry for the inconvenience. Use the coupon code '{Coupon}' to get 10% off your next order.", message.OrderId, coupon);

        return Task.CompletedTask;
    }
}