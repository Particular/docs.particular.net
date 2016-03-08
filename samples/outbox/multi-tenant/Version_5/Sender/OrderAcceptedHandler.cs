using System;
using NServiceBus;

public class OrderAcceptedHandler : IHandleMessages<OrderAccepted>
{
    public void Handle(OrderAccepted message)
    {
        Console.WriteLine("Order {0} accepted.", message.OrderId);
    }
}