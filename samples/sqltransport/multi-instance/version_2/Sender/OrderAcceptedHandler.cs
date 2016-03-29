using System;
using Messages;
using NServiceBus;

public class OrderAcceptedHandler : IHandleMessages<ClientOrderAccepted>
{
    IBus bus;

    public OrderAcceptedHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(ClientOrderAccepted message)
    {
        Console.WriteLine("Received ClientOrderAccepted for ID {0}", message.OrderId);
    }
}