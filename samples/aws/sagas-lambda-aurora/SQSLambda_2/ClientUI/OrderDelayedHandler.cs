using NServiceBus.Logging;

public class OrderDelayedHandler : IHandleMessages<OrderDelayed>
{
    readonly ILog log = LogManager.GetLogger<OrderDelayedHandler>();

    public Task Handle(OrderDelayed message, IMessageHandlerContext context)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var coupon = new string(
            Enumerable.Repeat(chars, 8)
                .Select(s => s[Random.Shared.Next(s.Length)])
                .ToArray());

        log.Info($"Order {message.OrderId} is slightly delayed. We are sorry for the inconvenience. Use the coupon code '{coupon}' to get 10% off your next order.");

        return Task.CompletedTask;
    }
}