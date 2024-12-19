using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class Handler2 :
    IHandleMessages<HandlerMessage>
{
    static ILog log = LogManager.GetLogger<Handler2>();
    static Random random = new Random();

    public Task Handle(HandlerMessage message, IMessageHandlerContext context)
    {
        var milliseconds = random.Next(100, 1000);
        log.InfoFormat("HandlerMessage received going to Task.Delay({0}ms)", milliseconds);
        return Task.Delay(milliseconds, context.CancellationToken);
    }
}
