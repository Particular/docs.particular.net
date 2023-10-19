using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderDelayedHandler : IHandleMessages<OrderDelayed>
{
    static readonly ILog Log = LogManager.GetLogger<OrderDelayedHandler>();

    public Task Handle(OrderDelayed message, IMessageHandlerContext context)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var coupon = new string(
            Enumerable.Repeat(chars, 8)
                .Select(s => s[Random.Shared.Next(s.Length)])
                .ToArray());

        Log.Info($"Order {message.OrderId} is slightly delayed. We are sorry for the inconvenience. Use the coupon code '{coupon}' to get 10% off your next order.");

        return Task.CompletedTask;
    }
}