using NServiceBus.Logging;

public class MyHandler : IHandleMessages<MyMessage>
{
    static readonly ILog log = LogManager.GetLogger<MyHandler>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.Warn("Hello from MyHandler");
        return Task.CompletedTask;
    }
}