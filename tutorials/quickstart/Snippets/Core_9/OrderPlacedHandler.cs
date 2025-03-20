#region OrderPlacedHandler

using System.Threading.Tasks;
using NServiceBus;
using Messages;
using Microsoft.Extensions.Logging;

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
            logger.LogInformation($"Shipping has received OrderPlaced, OrderId = {message.OrderId}");
            return Task.CompletedTask;
        }
    }
}

#endregion

namespace Messages
{
    public class OrderPlaced
        : IEvent
    {
        public string OrderId { get; set; }
    }
}