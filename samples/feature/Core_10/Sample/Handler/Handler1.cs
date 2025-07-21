using System;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class Handler1(ILogger<Handler1> logger) :
    IHandleMessages<HandlerMessage>
{
    static Random random = new Random();

    public Task Handle(HandlerMessage message, IMessageHandlerContext context)
    {
        var milliseconds = random.Next(100, 1000);
        logger.LogInformation("HandlerMessage received going to Task.Delay({DelayMs}ms)", milliseconds);
        return Task.Delay(milliseconds, context.CancellationToken);
    }
}
