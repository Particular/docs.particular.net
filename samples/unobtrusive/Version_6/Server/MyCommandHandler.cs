using System;
using System.Threading.Tasks;
using Commands;
using NServiceBus;

public class MyCommandHandler : IHandleMessages<MyCommand>
{
    public Task Handle(MyCommand message, IMessageHandlerContext context)
    {
        Console.WriteLine("Command received, id:" + message.CommandId);
        Console.WriteLine("EncryptedString:" + message.EncryptedString);
        return Task.FromResult(0);
    }
}