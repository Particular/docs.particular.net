using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class PlaceOrderHandler :
    IHandleMessages<PlaceOrder>
{
    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        log.Info($"Order {message.OrderId} accepted.");
        var orderAccepted = new OrderAccepted
        {
            OrderId = message.OrderId,
            Value = message.Value
        };
        return context.Publish(orderAccepted);
    }

    static ILog log = LogManager.GetLogger<PlaceOrderHandler>();
}