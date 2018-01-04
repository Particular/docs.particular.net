using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class PingHandler :
    IHandleMessages<Ping>
{
    static ILog log = LogManager.GetLogger<PingHandler>();

    public Task Handle(Ping message, IMessageHandlerContext context)
    {
        log.Info($"Ping {context.MessageId}");
        return context.Reply(new Pong());
    }
}