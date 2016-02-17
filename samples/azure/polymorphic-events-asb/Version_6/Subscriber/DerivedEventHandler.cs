using System.Threading.Tasks;
using Events;
using NServiceBus;
using NServiceBus.Logging;

public class DerivedEventHandler : IHandleMessages<DerivedEvent>
{
    static ILog logger = LogManager.GetLogger<DerivedEventHandler>();

    public Task Handle(DerivedEvent message, IMessageHandlerContext context)
    {
        logger.InfoFormat("Received DerivedEvent. EventId: {0} Data: {1}", message.EventId, message.Data);
        return Task.FromResult(0);
    }
}