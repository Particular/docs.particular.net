using System;
using System.Threading.Tasks;
using NServiceBus;

class SomeMessageHandler :
    IHandleMessages<SomeMessage>
{
    public Task Handle(SomeMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine($"SomeMessage received via gateway channel: {message.Contents}");
        return Task.CompletedTask;
    }
}