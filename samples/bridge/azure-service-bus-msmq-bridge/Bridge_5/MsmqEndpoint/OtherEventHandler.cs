using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared;

namespace MsmqEndpoint;

public class OtherEventHandler : IHandleMessages<OtherEvent>
{
    static ILog log = LogManager.GetLogger<OtherEventHandler>();

    public Task Handle(OtherEvent message, IMessageHandlerContext context)
    {
        log.Info($"Received OtherEvent: {message.Property}");
        log.Info("Conversation Done");
        return Task.CompletedTask;
    }
}