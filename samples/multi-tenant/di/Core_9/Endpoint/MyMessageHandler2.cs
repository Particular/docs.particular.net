using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

class MyMessageHandler2(IMySession session, ILogger<MyMessageHandler2> logger) :
    IHandleMessages<MyMessage>
{
    readonly IMySession session = session;

    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        await session.Store(new MyOtherEntity());

        logger.LogInformation($"{context.MessageId} got UOW instance {session.GetHashCode()}");
    }
}