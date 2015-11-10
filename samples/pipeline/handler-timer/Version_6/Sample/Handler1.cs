using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region handler
public class Handler1 : IHandleMessages<Message>
{
    static ILog logger = LogManager.GetLogger<Handler1>();
    static Random random = new Random();

    public async Task Handle(Message message, IMessageHandlerContext context)
    {
        int milliseconds = random.Next(100, 1000);
        logger.InfoFormat("Message received going to Task.Delay({0}ms)", milliseconds);
        await Task.Delay(milliseconds);
    }
}
#endregion