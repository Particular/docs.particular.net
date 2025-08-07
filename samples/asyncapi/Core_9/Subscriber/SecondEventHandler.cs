using NServiceBus;
using NServiceBus.Logging;

public class SecondEventHandler : IHandleMessages<SecondEvent>
{
    public Task Handle(SecondEvent message, IMessageHandlerContext context)
    {
        Log.Info("Received Second Event");
        return Task.CompletedTask;
    }

    private static ILog Log = LogManager.GetLogger<SecondEventHandler>();
}