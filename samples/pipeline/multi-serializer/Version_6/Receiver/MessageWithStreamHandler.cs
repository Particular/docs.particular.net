using System;
using System.Threading.Tasks;
using NServiceBus;

public class MessageHandler :
    IHandleMessages<MessageWithJson>,
    IHandleMessages<MessageWithBinary>
{
    public Task Handle(MessageWithJson message)
    {
        Console.WriteLine("Received JSON message with property '{0}'", message.SomeProperty);
        return Task.FromResult(0);
    }

    public Task Handle(MessageWithBinary message)
    {
        Console.WriteLine("Received Binary message with property '{0}'", message.SomeProperty);
        return Task.FromResult(0);
    }
}