using System;
using System.Threading.Tasks;
using NServiceBus;

public class MyMessageHandler : IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        return Task.CompletedTask;
    }
}
