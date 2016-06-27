using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region handler
public class Handler1 : IHandleMessages<Message>
{
    static ILog log = LogManager.GetLogger<Handler1>();
    static Random random = new Random();

    public Task Handle(Message message, IMessageHandlerContext context)
    {
        var milliseconds = random.Next(100, 1000);
        log.Info($"Message received going to Task.Delay({milliseconds}ms)");
        return Task.Delay(milliseconds);
    }
}
#endregion