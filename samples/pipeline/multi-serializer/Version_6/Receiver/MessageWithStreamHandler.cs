using System;
using System.Threading.Tasks;
using NServiceBus;

public class MessageHandler :
    IHandleMessages<MessageWithJson>,
    IHandleMessages<MessageWithXml>
{
    public Task Handle(MessageWithJson message, IMessageHandlerContext context)
    {
        Console.WriteLine("Received JSON message with property '{0}'", message.SomeProperty);
        return Task.FromResult(0);
    }

    public Task Handle(MessageWithXml message, IMessageHandlerContext context)
    {
        Console.WriteLine("Received XML message with property '{0}'", message.SomeProperty);
        return Task.FromResult(0);
    }
    
}