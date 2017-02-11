using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

namespace Billing
{
    #region SubscriberHandlerDontPublishOrderBilled

    public class OrderPlacedHandler :
        IHandleMessages<OrderPlaced>
    {
        static ILog logger = LogManager.GetLogger<OrderPlacedHandler>();

        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            logger.Info($"Received OrderPlaced, OrderId = {message.OrderId} - Charging credit card...");

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