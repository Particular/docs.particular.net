using System;
using System.Collections.Concurrent;
using NServiceBus;

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

        if (context.Headers.TryGetValue(Headers.Retries, out var numOfRetries))
        {
            Last.TryGetValue(message.Id, out var value);

            if (numOfRetries != value)
            {
                Console.WriteLine($"This is retry number {numOfRetries}");
                Last.AddOrUpdate(message.Id, numOfRetries, (key, oldValue) => numOfRetries);
            }
        }

        throw new Exception("An exception occurred in the handler.");
    }
}