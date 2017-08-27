using System.Threading.Tasks;
using Events;
using NServiceBus;
using NServiceBus.Logging;

public class DerivedEventHandler :
    IHandleMessages<DerivedEvent>
{
    static ILog log = LogManager.GetLogger<DerivedEventHandler>();

    public Task Handle(DerivedEvent message, IMessageHandlerContext context)
    {
        log.Info($"Received DerivedEvent. EventId: {message.EventId} Data: {message.Data}");
        return Task.CompletedTask;
    }
}