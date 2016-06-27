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

        var message = new OrderPlaced
        {
            OrderId = placeOrder.OrderId,
            WorkerName = Assembly.GetEntryAssembly().GetName().Name
        };
        log.Info($"Sent Order placed event for orderId [{placeOrder.OrderId}].");
        return context.Reply(message);
    }
}
