using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

public class MyHandler : IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        string format = string.Format("Received MyMessage. Property1:'{0}'. Property2:'{1}'", message.Property1, message.Property2);
        Console.WriteLine(format);
        return Task.FromResult(0);
    }
}