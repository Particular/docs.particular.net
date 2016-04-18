using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region OrderCreatedHandler
public class OrderCreatedHandler : IHandleMessages<OrderPlaced>
{
    static ILog log = LogManager.GetLogger<OrderCreatedHandler>();

    public Task Handle(OrderPlaced message, IMessageHandlerContext context)
    {
        log.InfoFormat(@"Handling: OrderPlaced for Order Id: {0}", message.OrderId);
        return Task.FromResult(0);
    }
}
#endregion