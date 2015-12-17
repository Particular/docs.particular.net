using System;
using System.Threading.Tasks;
using NServiceBus;

public class EventMessageHandler : IHandleMessages<EventMessage>
{
    public Task Handle(EventMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine("Subscriber 1 received EventMessage with Id {0}.", message.EventId);
        Console.WriteLine("Message time: {0}.", message.Time);
        Console.WriteLine("Message duration: {0}.", message.Duration);
        return Task.FromResult(0);
    }
}