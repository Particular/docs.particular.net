using System;
using NServiceBus;

public class MyHandler : IHandleMessages<MyMessage>
{
    IBus bus;

    public MyHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(MyMessage message)
    {
        Console.WriteLine("Got `MyMessage` with id: {0}, property value: {1}",bus.CurrentMessageContext.Id,message.SomeProperty);
    }
}