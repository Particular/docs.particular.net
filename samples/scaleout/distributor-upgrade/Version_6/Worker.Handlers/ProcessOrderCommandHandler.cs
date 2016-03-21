using NServiceBus;
using System.Reflection;
using System.Threading.Tasks;
using NServiceBus.Logging;

public class ProcessOrderCommandHandler : IHandleMessages<PlaceOrder>
{
    static ILog log = LogManager.GetLogger<ProcessOrderCommandHandler>();

    public Task Handle(PlaceOrder placeOrder, IMessageHandlerContext context)
    {
        // Process Order...
        log.Info("Processing received order....");

        OrderPlaced message = new OrderPlaced
        {
            OrderId = placeOrder.OrderId,
            WorkerName = Assembly.GetEntryAssembly().GetName().Name
        };
        log.InfoFormat("Sent Order placed event for orderId [{0}].", placeOrder.OrderId);
        return context.Reply(message);
    }
}
