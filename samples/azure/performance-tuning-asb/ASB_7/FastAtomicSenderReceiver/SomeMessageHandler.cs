using System.Threading.Tasks;
using NServiceBus;

public class SomeMessageHandler :
    IHandleMessages<SomeMessage>
{
    // maximum 100 for atomic sends
    public const int NumberOfMessagesToSend = 1;

    public async Task Handle(SomeMessage message, IMessageHandlerContext context)
    {
        Program.ReceiveCounter.OnNext(message);

        for (var i = 0; i < NumberOfMessagesToSend; i++)
        {
            await context.Send(new SomeMessage());
        }

    }
}