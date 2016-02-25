using System;
using System.Threading.Tasks;
using NServiceBus;

public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
{
    public Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        Console.WriteLine("Order {0} worth {1} submitted", message.OrderId, message.Value);
        return context.Reply(new OrderAccepted
        {
            OrderId = message.OrderId,
        });
    }
}
