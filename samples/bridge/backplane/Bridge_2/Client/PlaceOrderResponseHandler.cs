using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class PlaceOrderResponseHandler :
    IHandleMessages<PlaceOrderResponse>
{
    public Task Handle(PlaceOrderResponse message, IMessageHandlerContext context)
    {
        log.Info($"Order {message.OrderId} placed.");
        return Task.CompletedTask;
    }

    static ILog log = LogManager.GetLogger<PlaceOrderResponseHandler>();
}