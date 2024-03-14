using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region message-handlers

class MyMessageHandler1 :
    IHandleMessages<MyMessage>
{
    static readonly ILog log = LogManager.GetLogger<MyMessageHandler1>();
    readonly IMySession session;

    public MyMessageHandler1(IMySession session)
    {
        this.session = session;
    }

    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        await session.Store(new MyEntity());

        log.Info($"{context.MessageId} got UOW instance {session.GetHashCode()}");
    }
}

#endregion