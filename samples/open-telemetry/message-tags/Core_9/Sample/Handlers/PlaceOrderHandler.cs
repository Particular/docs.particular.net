using NServiceBus;
using System;
using System.Threading.Tasks;

class PlaceOrderHandler : IHandleMessages<PlaceOrder>
{
    public Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Order placed. OrderId={message.OrderId}, CustomerId={message.CustomerId}");
        return Task.CompletedTask;
    }
}
