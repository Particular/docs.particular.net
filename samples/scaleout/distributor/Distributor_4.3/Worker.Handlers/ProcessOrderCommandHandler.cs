using NServiceBus;
using System.Reflection;
using NServiceBus.Logging;

#region WorkerHandler

public class ProcessOrderCommandHandler : IHandleMessages<PlaceOrder>
{
    static ILog log = LogManager.GetLogger(typeof(ProcessOrderCommandHandler));
    IBus bus;

    public ProcessOrderCommandHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(PlaceOrder placeOrder)
    {
        // Process Order...
        log.Info("Processing received order....");

        var message = new OrderPlaced
        {
            OrderId = placeOrder.OrderId,
            WorkerName = Assembly.GetEntryAssembly().GetName().Name
        };
        bus.Reply(message);
        log.Info($"Sent Order placed event for orderId [{placeOrder.OrderId}].");
    }
}

#endregion
