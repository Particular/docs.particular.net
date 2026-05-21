using Messages;
using Microsoft.Extensions.Logging;

namespace Sales;

public class PlaceOrderHandler(ILogger<PlaceOrderHandler> logger) : IHandleMessages<PlaceOrder>
{
    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        if (Random.Shared.Next(0, 5) == 0)
        {
            throw new Exception("Oops");
        }

        logger.LogInformation("Received PlaceOrder, OrderId = {OrderId}", message.OrderId);
        return Task.CompletedTask;
    }
}