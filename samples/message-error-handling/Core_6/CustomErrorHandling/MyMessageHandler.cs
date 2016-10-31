using System;
using System.Threading.Tasks;
using NServiceBus;

public class MyMessageHandler :
    IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        if (message.ThrowCustomException)
        {
            throw new MyCustomException();
        }

        throw new Exception("An exception occurred in the handler.");
    }
}