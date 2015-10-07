using System;
using System.Threading.Tasks;
using NServiceBus;

public class CreateOrderHandler : IHandleMessages<CreateOrder>
{
    public Task Handle(CreateOrder message)
    {
        Console.WriteLine("Order received");
        return Task.FromResult(0);
    }
}
