using System;
using System.Threading.Tasks;
using NServiceBus;

public class MyHandler : IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        throw new Exception("The exception message");
    }
}