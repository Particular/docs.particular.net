using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class CancelOrderHandler :
    IHandleMessages<CancelOrder>
{
    public Task Handle(CancelOrder message, IMessageHandlerContext context)
    {
        log.Info($"CancelOrder command received: {message.OrderId}");
        return Task.CompletedTask;
    }

    static ILog log = LogManager.GetLogger<CancelOrderHandler>();
}