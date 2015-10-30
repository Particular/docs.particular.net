using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class Handler2 : IHandleMessages<Message>
{
    static ILog logger = LogManager.GetLogger<Handler2>();
    static Random random = new Random();

    public async Task Handle(Message message, IMessageHandlerContext context)
    {
        int milliseconds = random.Next(100, 1000);
        logger.InfoFormat("Message received going to Task.Delay({0}ms)", milliseconds);
        await Task.Delay(milliseconds);
    }
}
