using System;
using System.Threading.Tasks;
using NServiceBus;

public class AnotherEventMessageHandler : IHandleMessages<AnotherEventMessage>
{
    public Task Handle(AnotherEventMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine("Subscriber 1 received AnotherEventMessage with Id {0}.", message.EventId);
        Console.WriteLine("Message time: {0}.", message.Time);
        Console.WriteLine("Message duration: {0}.", message.Duration);
        return Task.FromResult(0);
    }
    
}