using System.Threading.Tasks;
using Shared.Messages.In.A.Deep.Nested.Namespace.Nested.Events;
using NServiceBus;
using NServiceBus.Logging;

public class SuperDuperEventHandler : IHandleMessages<SuperDuperEvent>
{
    static ILog logger = LogManager.GetLogger<SuperDuperEventHandler>();

    public Task Handle(SuperDuperEvent message, IMessageHandlerContext context)
    {
        logger.InfoFormat("Received SuperDuperEvent. EventId: {0}", message.EventId);
        return Task.FromResult(0);
    }
}