using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;

public class SomeMessageHandler :
    IHandleMessages<SomeMessage>
{
    // maximum 100 for atomic sends
    public const int NumberOfMessagesToSend = 1;

    public Task Handle(SomeMessage message, IMessageHandlerContext context)
    {
        Program.ReceiveCounter.OnNext(message);

        var tasks = new List<Task>();
        for (var i = 0; i < NumberOfMessagesToSend; i++)
        {
            tasks.Add(context.Send(new SomeMessage()));
        }
        return Task.WhenAll(tasks);
    }
}