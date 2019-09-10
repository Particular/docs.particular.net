using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class MyMessageHandler2 :
    IHandleMessages<MyMessage>
{
    ILog log = LogManager.GetLogger<MyMessageHandler2>();

    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        await context.Store(new MyOtherEntity())
            .ConfigureAwait(false);

        log.Info($"{context.MessageId} got UoW instance {context.GetSession().GetHashCode()}");
    }
}