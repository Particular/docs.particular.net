using NServiceBus;
using NServiceBus.Logging;

#region OrderCreatedHandler
public class OrderCreatedHandler : IHandleMessages<OrderPlaced>
{
    static ILog log = LogManager.GetLogger<OrderCreatedHandler>();

    public void Handle(OrderPlaced message)
    {
        log.InfoFormat(@"Handling: OrderPlaced for Order Id: {0}", message.OrderId);
    }
}
#endregion