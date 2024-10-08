using System.Threading.Tasks;
using Messages;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Shipping;

public class OrderBilledHandler(ILogger<OrderBilledHandler> logger) :
    IHandleMessages<OrderBilled>
{   
    public Task Handle(OrderBilled message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received OrderBilled, OrderId = {orderId} - Should we ship now?", message.OrderId);
        return Task.CompletedTask;
    }
}