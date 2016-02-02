using System;
using System.Threading.Tasks;
using NServiceBus;

public class OrderAcceptedHandler : IHandleMessages<OrderAccepted>
{
    public Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        Console.WriteLine("Order {0} accepted.", message.OrderId);
        return Task.FromResult(0);
    }
}