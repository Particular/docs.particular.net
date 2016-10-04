using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;

public class SomeMessageHandler :
    IHandleMessages<SomeMessage>
{
    public const int NumberOfMessagesToSend = 1; 

    public Task Handle(SomeMessage message, IMessageHandlerContext context)
    {
        Program.ReceiveCounter.OnNext(message);

        var list = new List<Task>();
        for (var i = 0; i < NumberOfMessagesToSend; i++)
        {
            list.Add(context.Send(new SomeMessage()));
        }
        return Task.WhenAll(list);
    }
}