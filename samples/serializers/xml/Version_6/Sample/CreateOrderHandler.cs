using System;
using System.Threading.Tasks;
using NServiceBus;
using XmlSample;

public class CreateOrderHandler : IHandleMessages<CreateOrder>
{
    public Task Handle(CreateOrder message)
    {
        Console.WriteLine("Order received");
        return Task.FromResult(0);
    }
}
