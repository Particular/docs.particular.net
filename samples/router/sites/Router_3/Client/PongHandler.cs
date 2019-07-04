using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class PongHandler :
    IHandleMessages<Pong>
{
    static ILog log = LogManager.GetLogger<PongHandler>();

    public Task Handle(Pong message, IMessageHandlerContext context)
    {
        log.Info($"Ping {context.MessageHeaders[Headers.CorrelationId]}");
        return Task.CompletedTask;
    }
}