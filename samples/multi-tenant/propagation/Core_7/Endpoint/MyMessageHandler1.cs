using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region message-handlers

class MyMessageHandler1 :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyMessageHandler1>();
    IMySession session;

    public MyMessageHandler1(IMySession session)
    {
        this.session = session;
    }

    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        await session.Store(new MyEntity())
            .ConfigureAwait(false);

        log.Info($"{context.MessageId} got UOW instance {session.GetHashCode()}");
    }
}

#endregion