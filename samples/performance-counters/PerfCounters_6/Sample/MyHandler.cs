using NServiceBus.Logging;

#region handler

public class MyHandler : IHandleMessages<MyMessage>
{
    static readonly ILog log = LogManager.GetLogger<MyHandler>();

    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var sleepTime = Random.Shared.Next(1, 1000);
        await Task.Delay(sleepTime, context.CancellationToken);
        log.Info($"Hello from MyHandler. Slept for {sleepTime} ms");
    }
}

#endregion