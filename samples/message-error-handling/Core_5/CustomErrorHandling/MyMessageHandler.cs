using System;
using NServiceBus;

public class MyMessageHandler :
    IHandleMessages<MyMessage>
{
    IBus bus;

    public MyMessageHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(MyMessage message)
    {
        if (message.ThrowCustomException)
        {
            throw new MyCustomException();
        }

        throw new Exception("An exception occurred in the handler.");
    }
}