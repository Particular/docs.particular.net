using System.Threading.Tasks;
using Messages;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Sales
{
    public class PlaceOrderHandler :
        IHandleMessages<PlaceOrder>
    {
        private readonly ILogger<PlaceOrderHandler> logger;

        public PlaceOrderHandler(ILogger<PlaceOrderHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            logger.LogInformation("Received PlaceOrder, OrderId = {orderId}", message.OrderId);
            return Task.CompletedTask;
        }
    }
}