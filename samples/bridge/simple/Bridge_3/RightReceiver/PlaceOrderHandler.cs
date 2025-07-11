using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class PlaceOrderHandler(ILogger<PlaceOrderHandler> logger) : IHandleMessages<PlaceOrder>
{
    public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
       logger.LogInformation("Received PlaceOrder Command with Id {OrderId}", message.OrderId);

        await context.Reply(new OrderResponse { OrderId = message.OrderId });
    }
}