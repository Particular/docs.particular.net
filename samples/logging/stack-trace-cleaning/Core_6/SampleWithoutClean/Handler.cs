using System;
using System.Threading.Tasks;
using NServiceBus;

public class Handler :
    IHandleMessages<Message>
{
    public Task Handle(Message message, IMessageHandlerContext context)
    {
        throw new Exception("Foo");
    }
}
