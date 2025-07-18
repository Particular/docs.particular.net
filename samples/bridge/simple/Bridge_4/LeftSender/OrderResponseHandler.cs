using System;
using System.Threading.Tasks;
using NServiceBus;

public class OrderResponseHandler : IHandleMessages<OrderResponse>
{
    public Task Handle(OrderResponse message, IMessageHandlerContext context)
    {
        Console.WriteLine($"OrderResponse Reply received with Id {message.OrderId}");

        return Task.CompletedTask;
    }
}