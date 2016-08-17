using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class MyMessageHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyMessageHandler>();
    static ConcurrentDictionary<Guid, string> Last = new ConcurrentDictionary<Guid, string>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.Info($"ReplyToAddress: {context.ReplyToAddress} MessageId:{context.MessageId}");

        string numOfRetries;
        if (context.MessageHeaders.TryGetValue(Headers.DelayedRetries, out numOfRetries))
        {
            string value;
            Last.TryGetValue(message.Id, out value);

            if (numOfRetries != value)
            {
                log.Info($"This is retry number {numOfRetries}");
                Last.AddOrUpdate(message.Id, numOfRetries, (key, oldValue) => numOfRetries);
            }
        }

        throw new Exception("An exception occurred in the handler.");
    }
}