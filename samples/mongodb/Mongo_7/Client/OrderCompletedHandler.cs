using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderCompletedHandler : IHandleMessages<OrderCompleted>
{
    static ILog logger = LogManager.GetLogger<OrderCompletedHandler>();

    public Task Handle(OrderCompleted message, IMessageHandlerContext context)
    {
        logger.Info($"Received OrderCompleted for OrderId {message.OrderId}");
        return Task.FromResult(0);
    }
}