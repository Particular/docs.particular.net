using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

#region message-handlers

class MyMessageHandler1(ILogger<MyMessageHandler1> logger) :
    IHandleMessages<MyMessage>
{
   public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        await context.Store(new MyEntity());

        logger.LogInformation($"{context.MessageId} got UoW instance {context.GetSession().GetHashCode()}");
    }
}

#endregion