using System;
using System.Threading.Tasks;
using NServiceBus;

public class MyHandler : IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine("Got `MyMessage` with id: {0}, property value: {1}", context.MessageId, message.SomeProperty);

        return Task.FromResult(0);
    }
}