using System;
using NServiceBus;

public class AnotherEventMessageHandler : IHandleMessages<AnotherEventMessage>
{
    public void Handle(AnotherEventMessage message)
    {
        Console.WriteLine("Subscriber 1 received AnotherEventMessage with Id {0}.", message.EventId);
        Console.WriteLine("Message time: {0}.", message.Time);
        Console.WriteLine("Message duration: {0}.", message.Duration);
    }
}