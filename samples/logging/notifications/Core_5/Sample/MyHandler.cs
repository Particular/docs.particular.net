using System;
using NServiceBus;

public class MyHandler :
    IHandleMessages<MyMessage>
{
    public void Handle(MyMessage message)
    {
        throw new Exception("The exception message");
    }
}