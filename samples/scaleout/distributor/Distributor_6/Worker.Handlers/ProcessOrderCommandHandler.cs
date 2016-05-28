using NServiceBus;
using System.Reflection;
using System.Threading.Tasks;
using NServiceBus.Logging;

#region WorkerHandler

public class ProcessOrderCommandHandler : IHandleMessages<PlaceOrder>
{
    static ILog log = LogManager.GetLogger<ProcessOrderCommandHandler>();

    public async Task Handle(PlaceOrder placeOrder, IMessageHandlerContext context)
    {
        // Process Order...
        log.Info("Processing received order....");

        var message = new OrderPlaced
        {
            OrderId = placeOrder.OrderId,
            WorkerName = Assembly.GetEntryAssembly().GetName().Name
        };
        await context.Reply(message)
            .ConfigureAwait(false);
        log.InfoFormat($"Sent Order placed event for orderId [{placeOrder.OrderId}].");
    }
}

#endregion
