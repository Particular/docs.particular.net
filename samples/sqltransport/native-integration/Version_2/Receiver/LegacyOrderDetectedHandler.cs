using System;
using NServiceBus;

public class LegacyOrderDetectedHandler : IHandleMessages<LegacyOrderDetected>
{
    public void Handle(LegacyOrderDetected message)
    {
        Console.WriteLine("Legacy order with id {0} detected", message.OrderId);

        //Get the order details from the database and publish an event
    }
}