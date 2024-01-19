using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region message-handlers

class MyMessageHandler1 :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyMessageHandler1>();

    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        await context.Store(new MyEntity())
            .ConfigureAwait(false);

        log.Info($"{context.MessageId} got UoW instance {context.GetSession().GetHashCode()}");
    }
}

#endregion