using System;
using System.Threading.Tasks;
using NServiceBus;

#region handler
public class Handler : IHandleMessages<Message>
{
    public Task Handle(Message message, IMessageHandlerContext context)
    {
        throw new Exception("Foo");
    }
}
#endregion