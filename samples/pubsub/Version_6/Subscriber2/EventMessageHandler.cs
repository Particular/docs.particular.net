using System;
using System.Threading.Tasks;
using NServiceBus;

public class EventMessageHandler : IHandleMessages<IMyEvent>
{
    public Task Handle(IMyEvent message, IMessageHandlerContext context)
    {
        Console.WriteLine("Subscriber 2 received IEvent with Id {0}.", message.EventId);
        Console.WriteLine("Message time: {0}.", message.Time);
        Console.WriteLine("Message duration: {0}.", message.Duration);
        return Task.FromResult(0);
    }

}