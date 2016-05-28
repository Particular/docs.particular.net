using System.Threading.Tasks;
using Events;
using NServiceBus;
using NServiceBus.Logging;

public class DerivedEventHandler : IHandleMessages<DerivedEvent>
{
    static ILog logger = LogManager.GetLogger<DerivedEventHandler>();

    public Task Handle(DerivedEvent message, IMessageHandlerContext context)
    {
        logger.Info($"Received DerivedEvent. EventId: {message.EventId} Data: {message.Data}");
        return Task.FromResult(0);
    }
}