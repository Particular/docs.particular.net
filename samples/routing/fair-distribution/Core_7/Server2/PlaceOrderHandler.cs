using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class PlaceOrderHandler :
    IHandleMessages<PlaceOrder>
{
    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        Thread.Sleep(2000);
        log.Info($"Order {message.OrderId} accepted.");
        return Task.CompletedTask;
    }

    static ILog log = LogManager.GetLogger<PlaceOrderHandler>();
}