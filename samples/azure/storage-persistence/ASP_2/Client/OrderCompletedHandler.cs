using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderCompletedHandler :
    IHandleMessages<OrderCompleted>
{
    static ILog log = LogManager.GetLogger<OrderCompletedHandler>();

    public Task Handle(OrderCompleted message, IMessageHandlerContext context)
    {
        log.Info($"Received OrderCompleted for OrderId {message.OrderId}");
        return Task.CompletedTask;
    }
}