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

        logger.LogInformation("{MessageId} got UOW instance {SessionHash}", context.MessageId, session.GetHashCode());
    }
}