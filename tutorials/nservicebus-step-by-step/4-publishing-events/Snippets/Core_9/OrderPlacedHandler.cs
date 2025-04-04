using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Billing
{
    #region SubscriberHandlerDontPublishOrderBilled

    public class OrderPlacedHandler(ILogger<OrderPlacedHandler> logger) :
        IHandleMessages<OrderPlaced>
    {
        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            logger.LogInformation("Received OrderPlaced, charging OrderId: {orderId}", message.OrderId);

            return Task.CompletedTask;
        }
    }

    #endregion

    public class OrderPlaced :
        IEvent
    {
        public string OrderId { get; set; }
    }
}
