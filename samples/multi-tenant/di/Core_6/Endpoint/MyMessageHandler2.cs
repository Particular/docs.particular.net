using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class MyMessageHandler2 :
    IHandleMessages<MyMessage>
{
    ILog log = LogManager.GetLogger<MyMessageHandler2>();
    IMySession session;

    public MyMessageHandler2(IMySession session)
    {
        this.session = session;
    }

    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        await session.Store(new MyOtherEntity())
            .ConfigureAwait(false);

        log.Info($"{context.MessageId} got UOW instance {session.GetHashCode()}");
    }
}