using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

public class OrderAcceptedHandler :
    IHandleMessages<ClientOrderAccepted>
{
    public Task Handle(ClientOrderAccepted message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Received ClientOrderAccepted for ID {message.OrderId}");
        return Task.FromResult(0);
    }
}