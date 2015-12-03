using System;
using Commands;
using NServiceBus;

public class MyCommandHandler : IHandleMessages<MyCommand>
{
    IBus bus;

    public MyCommandHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(MyCommand message)
    {
        Console.WriteLine("Command received, id:" + message.CommandId);
        Console.WriteLine("EncryptedString:" + message.EncryptedString);
    }
}