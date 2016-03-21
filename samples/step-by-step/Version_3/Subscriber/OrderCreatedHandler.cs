using log4net;
using NServiceBus;
#region OrderCreatedHandler
public class OrderCreatedHandler : IHandleMessages<OrderPlaced>
{
    static ILog log = LogManager.GetLogger(typeof(OrderCreatedHandler));

    public void Handle(OrderPlaced message)
    {
        log.InfoFormat(@"Handling: OrderPlaced for Order Id: {0}", message.OrderId);
    }
}
#endregion