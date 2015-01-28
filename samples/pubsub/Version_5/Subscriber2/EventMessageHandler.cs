using System;
using NServiceBus;
    using MyMessages;

public class EventMessageHandler : IHandleMessages<IMyEvent>
{
    public void Handle(IMyEvent message)
    {
        Console.WriteLine(string.Format("Subscriber 2 received IEvent with Id {0}.", message.EventId));
        Console.WriteLine(string.Format("Message time: {0}.", message.Time));
        Console.WriteLine(string.Format("Message duration: {0}.", message.Duration));
        Console.WriteLine("==========================================================================");
    }
}