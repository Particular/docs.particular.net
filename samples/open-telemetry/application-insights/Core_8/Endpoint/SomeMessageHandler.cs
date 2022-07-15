using System;
using System.Threading.Tasks;
using NServiceBus;

class SomeMessageHandler : IHandleMessages<SomeMessage>
{
    public Task Handle(SomeMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine("Message handled");
        return Task.CompletedTask;
    }
}