using System;
using Messages;
using NServiceBus;

public class MyHandler : IHandleMessages<MyMessage>
{
    public void Handle(MyMessage message)
    {
        string format = string.Format("Received MyMessage. Property1:'{0}'. Property2:'{1}'", message.Property1, message.Property2);
        Console.WriteLine(format);
    }
}