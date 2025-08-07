using NServiceBus;
using NServiceBus.Logging;

public class FirstEventHandler : IHandleMessages<FirstEvent>
{
    public Task Handle(FirstEvent message, IMessageHandlerContext context)
    {
        Log.Info("Received First Event");
        return Task.CompletedTask;
    }

    private static ILog Log = LogManager.GetLogger<FirstEventHandler>();
}