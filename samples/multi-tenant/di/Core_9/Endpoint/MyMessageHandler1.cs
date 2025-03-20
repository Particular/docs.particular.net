using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
#region message-handlers

class MyMessageHandler1(IMySession session, ILogger<MyMessageHandler1> logger) :
    IHandleMessages<MyMessage>
{
      readonly IMySession session = session;

    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        await session.Store(new MyEntity());

        logger.LogInformation($"{context.MessageId} got UOW instance {session.GetHashCode()}");
    }
}

#endregion