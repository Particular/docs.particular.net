using System;
using System.Collections.Concurrent;
using NServiceBus;

#region Handler
public class MyMessageHandler :
    IHandleMessages<MyMessage>
{
    IBus bus;
    static ConcurrentDictionary<Guid, string> Last = new ConcurrentDictionary<Guid, string>();

    public MyMessageHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(MyMessage message)
    {
        var context = bus.CurrentMessageContext;
        Console.WriteLine($"Handling {nameof(MyMessage)} with MessageId:{context.Id}");
        throw new Exception("An exception occurred in the handler.");
    }
}
#endregion