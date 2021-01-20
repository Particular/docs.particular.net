using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class MyCommandHandler : IHandleMessages<MyCommand>
{
    internal static long ProcessedCount;
    static ILog log = LogManager.GetLogger<MyCommandHandler>();

    public Task Handle(MyCommand commandMessage, IMessageHandlerContext context)
    {
        var random = new Random();
        var value = random.Next(100);

        Interlocked.Increment(ref MyCommandHandler.ProcessedCount);


        if (value <= 5)
        {
            throw new Exception("5% chance of message failure " + Guid.NewGuid().ToString());
        }

        log.Info($"Hello from {nameof(MyCommandHandler)}");
        return Task.CompletedTask;
    }
}