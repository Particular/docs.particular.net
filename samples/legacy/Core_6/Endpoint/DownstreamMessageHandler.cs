using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class DownstreamMessageHandler : IHandleMessages<DownstreamMessage>
{
    static ILog log = LogManager.GetLogger<DownstreamMessageHandler>();

    public Task Handle(DownstreamMessage message, IMessageHandlerContext context)
    {
        log.Info($"Got message with culture {Thread.CurrentThread.CurrentCulture.Name} and user {Thread.CurrentPrincipal.Identity.Name}");
        return Task.CompletedTask;
    }
}