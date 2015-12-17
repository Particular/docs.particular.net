using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using NServiceBus;
#region Handler
public class MyMessageHandler : IHandleMessages<MyMessage>
{
    static ConcurrentDictionary<Guid, string> Last = new ConcurrentDictionary<Guid, string>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine($"ReplyToAddress: {context.ReplyToAddress} MessageId:{context.MessageId}");
        
        string numOfRetries;
        if (context.MessageHeaders.TryGetValue(Headers.Retries, out numOfRetries))
        {
            string value;
            Last.TryGetValue(message.Id, out value);

            if (numOfRetries != value)
            {
                Console.WriteLine("This is second level retry number {0}", numOfRetries);
                Last.AddOrUpdate(message.Id, numOfRetries, (key, oldValue) => numOfRetries);
            }
        }

        throw new Exception("An exception occurred in the handler.");
    }
    
}
#endregion