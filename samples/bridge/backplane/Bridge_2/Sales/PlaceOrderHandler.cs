using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class PlaceOrderHandler :
    IHandleMessages<PlaceOrder>
{
    public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        log.Info($"Order {message.OrderId} accepted.");

        await context.Publish(new OrderAccepted
        {
            OrderId = message.OrderId,
            Value = message.Value
        }).ConfigureAwait(false);

        await context.Reply(new PlaceOrderResponse
        {
            OrderId = message.OrderId
        });
    }

    static ILog log = LogManager.GetLogger<PlaceOrderHandler>();
}