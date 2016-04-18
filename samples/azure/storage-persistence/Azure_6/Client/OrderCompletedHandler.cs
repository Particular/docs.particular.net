using NServiceBus;
using NServiceBus.Logging;

public class OrderCompletedHandler : IHandleMessages<OrderCompleted>
{
    static ILog logger = LogManager.GetLogger<OrderCompletedHandler>();
    
    public void Handle(OrderCompleted message)
    {
        logger.InfoFormat("Received OrderCompleted for OrderId {0}", message.OrderId);
    }
}