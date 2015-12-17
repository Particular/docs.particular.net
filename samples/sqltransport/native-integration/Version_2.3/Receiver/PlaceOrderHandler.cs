using System;
using System.Threading.Tasks;
using NServiceBus;

public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
{
    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        Console.WriteLine("Order {0} placed", message.OrderId);
        return Task.FromResult(0);
    }

}