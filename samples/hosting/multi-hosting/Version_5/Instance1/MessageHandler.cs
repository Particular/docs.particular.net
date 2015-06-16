using System;
using NServiceBus;

public class MessageHandler : IHandleMessages<MyMessage>
{
    public void Handle(MyMessage message)
    {
        Console.WriteLine("Hello from Instance 1");
    }
}