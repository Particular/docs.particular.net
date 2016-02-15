using NServiceBus;
using System;
using System.Reflection;
using System.Threading.Tasks;

#region WorkerHandler

public class ProcessOrderCommandHandler : IHandleMessages<PlaceOrder>
{
    public async Task Handle(PlaceOrder placeOrder, IMessageHandlerContext context)
    {
        // Process Order...
        Console.WriteLine("Processing received order....");

        OrderPlaced message = new OrderPlaced
        {
            OrderId = placeOrder.OrderId,
            WorkerName = Assembly.GetEntryAssembly().GetName().Name
        };
        await context.Reply(message);
        Console.WriteLine("Sent Order placed event for orderId [{0}].", placeOrder.OrderId);
    }
}

#endregion
