using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class PongHandler : IHandleMessages<Pong>
{
    static ILog log = LogManager.GetLogger<PongHandler>();
    
    public Task Handle(Pong message, IMessageHandlerContext context)
    {
        log.Info($"Ping {message.Payload}!");
        return Task.FromResult(0);
    }
}