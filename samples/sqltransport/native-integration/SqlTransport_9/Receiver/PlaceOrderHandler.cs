using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

namespace Receiver;

public class PlaceOrderHandler(ILogger<LegacyOrderDetectedHandler> logger) :
    IHandleMessages<PlaceOrder>
{
    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Order {OrderId} placed", message.OrderId);
        return Task.CompletedTask;
    }
}