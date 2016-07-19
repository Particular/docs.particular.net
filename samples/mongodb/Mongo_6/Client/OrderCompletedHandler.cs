using NServiceBus;
using NServiceBus.Logging;

public class OrderCompletedHandler :
    IHandleMessages<OrderCompleted>
{
    static ILog log = LogManager.GetLogger<OrderCompletedHandler>();

    public void Handle(OrderCompleted message)
    {
        log.Info($"Received OrderCompleted for OrderId {message.OrderId}");
    }
}