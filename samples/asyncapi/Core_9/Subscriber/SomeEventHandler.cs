using NServiceBus;
using NServiceBus.Logging;

public class SomeEventHandler : IHandleMessages<SomeEvent>
{
    public Task Handle(SomeEvent message, IMessageHandlerContext context)
    {
        Log.Info("Received Some Event");
        return Task.CompletedTask;
    }

    private static ILog Log = LogManager.GetLogger<SomeEventHandler>();
}