using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;

class LegacyBus : IBus
{
    List<Func<IMessageProcessingContext, Task>> operations = new List<Func<IMessageProcessingContext, Task>>();
    public IReadOnlyDictionary<string, string> Headers { get; private set; }
    public string MessageId { get; private set; }

    public void SendLocal(object message)
    {
        operations.Add(c => c.SendLocal(message));
    }

    public async Task DispatchMessages(IMessageProcessingContext context)
    {
        foreach (var operation in operations)
        {
            await operation(context).ConfigureAwait(false);
        }
    }

    public void Initialize(string messageId, IReadOnlyDictionary<string, string> messageHeaders)
    {
        Headers = messageHeaders;
        MessageId = messageId;
    }
}