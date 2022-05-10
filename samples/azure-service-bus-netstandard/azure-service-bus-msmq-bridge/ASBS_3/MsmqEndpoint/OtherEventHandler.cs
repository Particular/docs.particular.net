using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OtherEventHandler : IHandleMessages<OtherEvent>
{
    static ILog log = LogManager.GetLogger<OtherEventHandler>();

    public Task Handle(OtherEvent message, IMessageHandlerContext context)
    {
        log.Info($"Received OtherEvent: {message.Property}");
        return Task.CompletedTask;
    }
}