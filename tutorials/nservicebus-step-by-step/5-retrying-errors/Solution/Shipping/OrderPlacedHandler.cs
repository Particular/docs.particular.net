using System.Threading.Tasks;
using Messages;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Shipping
{
    public class OrderPlacedHandler :
        IHandleMessages<OrderPlaced>
    {
        private readonly ILogger<OrderPlacedHandler> logger;

        public OrderPlacedHandler(ILogger<OrderPlacedHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            logger.LogInformation("Received OrderPlaced, OrderId = {orderId} - Should we ship now?", message.OrderId);
            return Task.CompletedTask;
        }
    }
}