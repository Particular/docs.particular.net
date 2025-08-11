using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

class MyMessageHandler2 (ILogger<MyMessageHandler2> logger):
    IHandleMessages<MyMessage>
{
     public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        await context.Store(new MyOtherEntity());

        logger.LogInformation("{MessageId} got UoW instance {SessionHash}", context.MessageId, context.GetSession().GetHashCode());
    }
}