using System.Threading.Tasks;
using Messages;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Shipping
{
    public class OrderBilledHandler :
        IHandleMessages<OrderBilled>
    {
        private readonly ILogger<OrderBilledHandler> logger;

        public OrderBilledHandler(ILogger<OrderBilledHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            logger.LogInformation("Received OrderBilled, OrderId = {orderId} - Should we ship now?", message.OrderId);
            return Task.CompletedTask;
        }
    }
}