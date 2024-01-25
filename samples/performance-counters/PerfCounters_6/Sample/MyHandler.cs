using NServiceBus.Logging;

#region handler

public class MyHandler :
    IHandleMessages<MyMessage>
{
    static readonly ILog log = LogManager.GetLogger<MyHandler>();

    static readonly Random random = new Random();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var sleepTime = random.Next(1, 1000);
        log.Info($"Hello from MyHandler. Slept for {sleepTime}ms");
        return Task.Delay(sleepTime, context.CancellationToken);
    }
}

#endregion