using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared.Messages.In.A.Deep.Nested.Namespace.Nested.Events;

public class SomeEventHandler :
    IHandleMessages<SomeEvent>
{
    static ILog log = LogManager.GetLogger<SomeEventHandler>();

    public Task Handle(SomeEvent message, IMessageHandlerContext context)
    {
        log.Info($"Received SomeEvent. EventId: {message.EventId}");
        return Task.CompletedTask;
    }
}