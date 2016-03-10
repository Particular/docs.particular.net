using NServiceBus;
using System;
using System.Reflection;
using System.Threading.Tasks;

public class ProcessOrderCommandHandler : IHandleMessages<PlaceOrder>
{
    public Task Handle(PlaceOrder placeOrder, IMessageHandlerContext context)
    {
        // Process Order...
        Console.WriteLine("Processing received order....");

        OrderPlaced message = new OrderPlaced
        {
            OrderId = placeOrder.OrderId,
            WorkerName = Assembly.GetEntryAssembly().GetName().Name
        };
        Console.WriteLine("Sent Order placed event for orderId [{0}].", placeOrder.OrderId);
        return context.Reply(message);
    }
}
