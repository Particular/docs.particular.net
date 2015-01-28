using System;
using MyMessages.Other;
using NServiceBus;

public class AnotherEventMessageHandler : IHandleMessages<AnotherEventMessage>
{
    public void Handle(AnotherEventMessage message)
    {
        Console.WriteLine(string.Format("Subscriber 1 received AnotherEventMessage with Id {0}.", message.EventId));
        Console.WriteLine(string.Format("Message time: {0}.", message.Time));
        Console.WriteLine(string.Format("Message duration: {0}.", message.Duration));
        Console.WriteLine("==========================================================================");
    }
}