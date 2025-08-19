using System;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class Handler2(ILogger<Handler2> logger) :
    IHandleMessages<HandlerMessage>
{
     static Random random = new Random();

    public Task Handle(HandlerMessage message, IMessageHandlerContext context)
    {
        var milliseconds = random.Next(100, 1000);
        logger.LogInformation($"HandlerMessage received going to Task.Delay({milliseconds}ms)");
        return Task.Delay(milliseconds, context.CancellationToken);
    }
}
