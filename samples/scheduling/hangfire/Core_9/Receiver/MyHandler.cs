using System;
using System.Threading.Tasks;
using NServiceBus;

public class MyHandler :
    IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine("Hello from MyHandler");
        return Task.CompletedTask;
    }
}