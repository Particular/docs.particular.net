using System;
using MyMessages;
using NServiceBus;

public class EventMessageHandler : IHandleMessages<EventMessage>
{
    public void Handle(EventMessage message)
    {
        Console.WriteLine(string.Format("Subscriber 1 received EventMessage with Id {0}.", message.EventId));
        Console.WriteLine(string.Format("Message time: {0}.", message.Time));
        Console.WriteLine(string.Format("Message duration: {0}.", message.Duration));
        Console.WriteLine("==========================================================================");
    }
}