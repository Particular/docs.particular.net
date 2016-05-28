using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class Handler2 : IHandleMessages<Message>
{
    static ILog logger = LogManager.GetLogger<Handler2>();
    static Random random = new Random();

    public Task Handle(Message message, IMessageHandlerContext context)
    {
        var milliseconds = random.Next(100, 1000);
        logger.Info($"Message received going to Task.Delay({milliseconds}ms)");
        return Task.Delay(milliseconds);
    }
}
