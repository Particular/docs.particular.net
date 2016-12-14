using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using NServiceBus;

#region Handler
public class MyMessageHandler :
    IHandleMessages<MyMessage>
{
    static ConcurrentDictionary<Guid, string> Last = new ConcurrentDictionary<Guid, string>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Handling {nameof(MyMessage)} with MessageId:{context.MessageId}");
        throw new Exception("An exception occurred in the handler.");
    }

}
#endregion