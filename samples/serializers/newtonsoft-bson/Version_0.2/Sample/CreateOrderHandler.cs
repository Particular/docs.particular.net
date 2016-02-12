using System;
using NServiceBus;
public class CreateOrderHandler : IHandleMessages<CreateOrder>
{
    public void Handle(CreateOrder message)
    {
        Console.WriteLine("Order received");
    }
}
