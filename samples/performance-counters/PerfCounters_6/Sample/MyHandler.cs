using Microsoft.Extensions.Logging;

#region handler

public class MyHandler(ILogger<MyHandler> logger) : IHandleMessages<MyMessage>
{
    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var sleepTime = Random.Shared.Next(1, 1000);
        await Task.Delay(sleepTime, context.CancellationToken);
        logger.LogInformation("Hello from MyHandler. Slept for {SleepTime} ms", sleepTime);
    }
}

#endregion