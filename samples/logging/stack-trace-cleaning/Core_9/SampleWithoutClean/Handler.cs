using System;
using System.Threading.Tasks;
using NServiceBus;

// Extra line to make the exception throw on the same line as the other sample
public class Handler :
    IHandleMessages<Message>
{
    public Task Handle(Message message, IMessageHandlerContext context)
    {
        throw new Exception("Foo");
    }
}
