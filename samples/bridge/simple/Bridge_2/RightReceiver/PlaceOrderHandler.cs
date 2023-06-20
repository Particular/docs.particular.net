using System;
using System.Threading.Tasks;
using NServiceBus;

public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
{
    public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Received PlaceOrder Command with Id {message.OrderId}");

        await context.Reply(new OrderResponse { OrderId = message.OrderId }).ConfigureAwait(false);
    }
}