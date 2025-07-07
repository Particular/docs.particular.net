using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class OrderResponseHandler(ILogger<OrderResponseHandler> logger) : IHandleMessages<OrderResponse>
{
    public Task Handle(OrderResponse message, IMessageHandlerContext context)
    {
        logger.LogInformation("OrderResponse Reply received with Id {OrderId}", message.OrderId);
        return Task.CompletedTask;
    }
}