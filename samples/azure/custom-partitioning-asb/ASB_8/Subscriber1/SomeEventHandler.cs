using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

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