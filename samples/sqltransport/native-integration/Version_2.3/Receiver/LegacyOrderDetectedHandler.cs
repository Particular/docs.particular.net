using System;
using System.Threading.Tasks;
using NServiceBus;

public class LegacyOrderDetectedHandler : IHandleMessages<LegacyOrderDetected>
{
    public Task Handle(LegacyOrderDetected message, IMessageHandlerContext context)
    {
        Console.WriteLine("Legacy order with id {0} detected", message.OrderId);

        //Get the order details from the database and publish an event

        return Task.FromResult(0);
    }

}