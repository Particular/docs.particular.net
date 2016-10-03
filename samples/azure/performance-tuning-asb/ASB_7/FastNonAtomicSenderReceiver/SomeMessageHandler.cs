using System.Threading.Tasks;
using NServiceBus;

public class SomeMessageHandler :
    IHandleMessages<SomeMessage>
{
    public const int NumberOfMessagesToSend = 1000; 

    public async Task Handle(SomeMessage message, IMessageHandlerContext context)
    {
        Program.ReceiveCounter.OnNext(message);

        for (var i = 0; i < NumberOfMessagesToSend; i++)
        {
            await context.Send(new SomeMessage());
        }

    }
}