using System.Threading.Tasks;
using Messages;
using NServiceBus;
using Microsoft.Extensions.Logging;

namespace Shipping
{
    public class OrderPlacedHandler(ILogger<OrderPlacedHandler> logger) :
        IHandleMessages<OrderPlaced>
    {

        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            logger.LogInformation("Received OrderPlaced, OrderId = {OrderId} - Should we ship now?", message.OrderId);
            return Task.CompletedTask;
        }
    }
}