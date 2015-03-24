using System;
using NServiceBus;

public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
{
    public void Handle(PlaceOrder message)
    {
        Console.WriteLine("Order {0} placed", message.OrderId);
    }
}