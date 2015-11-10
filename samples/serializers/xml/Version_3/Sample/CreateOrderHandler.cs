using System;
using NServiceBus;
using XmlSample;

public class CreateOrderHandler : IHandleMessages<CreateOrder>
{
    public void Handle(CreateOrder message)
    {
        Console.WriteLine("Order received");
    }
}
