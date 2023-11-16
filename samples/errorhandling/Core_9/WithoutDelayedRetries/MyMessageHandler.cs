using System;
using System.Threading.Tasks;
using NServiceBus;

#region Handler
public class MyMessageHandler :
    IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Handling {nameof(MyMessage)} with MessageId:{context.MessageId}");
        throw new Exception("An exception occurred in the handler.");
    }
}
#endregion
