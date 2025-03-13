using System.Threading.Tasks;
using Messages;
using NServiceBus;
using Microsoft.Extensions.Logging;

namespace Shipping
{
    public class OrderBilledHandler(ILogger<OrderBilledHandler> logger) :
        IHandleMessages<OrderBilled>
    {
        public Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            logger.LogInformation($"Received OrderBilled, OrderId = {message.OrderId} - Should we ship now?");
            return Task.CompletedTask;
        }
    }
}