using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using NServiceBus;

public class MyMessageHandler :
    IHandleMessages<MyMessage>
{
    static ConcurrentDictionary<Guid, string> Last = new ConcurrentDictionary<Guid, string>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Handling {nameof(MyMessage)} with MessageId:{context.MessageId}");

        if (context.MessageHeaders.TryGetValue(Headers.DelayedRetries, out var numOfRetries))
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