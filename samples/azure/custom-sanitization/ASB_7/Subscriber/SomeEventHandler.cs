using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared.Messages.In.A.Deep.Nested.Namespace.Nested.Events;

public class SomeEventHandler : IHandleMessages<SomeEvent>
{
    static ILog logger = LogManager.GetLogger<SomeEventHandler>();

    public Task Handle(SomeEvent message, IMessageHandlerContext context)
    {
        logger.InfoFormat("Received SomeEvent. EventId: {0}", message.EventId);
        return Task.FromResult(0);
    }
}