using System;
using NServiceBus;

public class MyHandler : IHandleMessages<MyMessage>
{
    public void Handle(MyMessage message)
    {
        Console.WriteLine("Hello from MyHandler");
    }
}
