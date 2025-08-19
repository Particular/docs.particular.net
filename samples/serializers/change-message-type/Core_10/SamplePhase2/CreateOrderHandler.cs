using System;
using System.Threading.Tasks;
using NServiceBus;

public class CreateOrderHandler :
    IHandleMessages<CreateOrderPhase2>
{
    public Task Handle(CreateOrderPhase2 message, IMessageHandlerContext context)
    {
        Console.WriteLine("Phase 2 Order received");
        return Task.CompletedTask;
    }
}