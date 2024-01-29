using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class PlaceOrderHandler :
    IHandleMessages<PlaceOrder>
{
    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        log.Info($"PlaceOrder command received: {message.OrderId} {message.Value}");
        return Task.CompletedTask;
    }

    static ILog log = LogManager.GetLogger<PlaceOrderHandler>();
}