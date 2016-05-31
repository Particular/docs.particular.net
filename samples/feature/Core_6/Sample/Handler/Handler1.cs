using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class Handler1 : IHandleMessages<HandlerMessage>
{
    static ILog logger = LogManager.GetLogger<Handler1>();
    static Random random = new Random();

    public Task Handle(HandlerMessage message, IMessageHandlerContext context)
    {
        var milliseconds = random.Next(100, 1000);
        logger.Info($"HandlerMessage received going to Task.Delay({milliseconds}ms)");
        return Task.Delay(milliseconds);
    }
}
